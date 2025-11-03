namespace PetCare.Infrastructure.Payments;

using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;
using PetCare.Infrastructure.Options;

/// <summary>
/// Provides high-level methods for interacting with the LiqPay payment gateway, including building checkout requests
/// and querying payment status. This client encapsulates authentication, request formatting, and endpoint communication
/// for LiqPay operations.
/// </summary>
/// <remarks>Use this client to initiate payments or subscriptions via LiqPay and to check the status of existing
/// orders. The client is intended to be registered and resolved via dependency injection, and requires valid LiqPay API
/// credentials. All requests are sent using the configured HTTP client and are signed according to LiqPay's security
/// requirements. This class is thread-safe and can be used concurrently across multiple requests.</remarks>
public sealed class LiqPayClient : ILiqPayClient
{
    private readonly LiqPaySettings settings;
    private readonly HttpClient http;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiqPayClient"/> class using the specified settings and HTTP client factory.
    /// </summary>
    /// <param name="options">The options containing configuration settings for LiqPay integration. Must not be null.</param>
    /// <param name="httpFactory">The factory used to create HTTP clients for communicating with the LiqPay API. Must not be null.</param>
    public LiqPayClient(IOptions<LiqPaySettings> options, IHttpClientFactory httpFactory)
    {
        this.settings = options.Value;
        this.http = httpFactory.CreateClient(nameof(LiqPayClient));
        this.http.BaseAddress = new Uri(this.settings.ApiBase.TrimEnd('/') + "/");
    }

    /// <summary>
    /// Builds and prepares a LiqPay checkout payload for payment or subscription, returning the data required to
    /// initiate the checkout process.
    /// </summary>
    /// <remarks>The returned payload supports both one-time payments and recurring monthly subscriptions,
    /// depending on the input parameters. The result URL is constructed to provide maximum context for frontend UX. The
    /// method generates a unique order ID for each checkout request.</remarks>
    /// <param name="input">The details of the checkout to create, including payment amount, currency, description, and subscription
    /// options.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="LiqPayCheckoutResponseDto"/> containing the encoded checkout data, signature, public key, gateway
    /// URL, order ID, and result URL required for LiqPay integration.</returns>
    public async Task<LiqPayCheckoutResponseDto> BuildCheckoutAsync(CreateLiqPayCheckoutDto input, CancellationToken ct = default)
    {
        var orderId = BuildCompositeOrderId(
                scope: input.Scope,
                entityId: input.EntityId,
                isRecurring: input.IsRecurring,
                userId: input.UserId,
                anonymous: input.Anonymous);

        // Максимум параметрів у result_url для фронту (UX)
        var resultUrl = this.BuildRichResultUrl(input, orderId);

        // Параметри LiqPay pay/subscribe
        var payload = new Dictionary<string, object?>
        {
            ["version"] = 3,
            ["public_key"] = this.settings.PublicKey,
            ["action"] = input.IsRecurring ? "subscribe" : "pay",
            ["amount"] = input.Amount,
            ["currency"] = input.Currency,
            ["description"] = input.Description ?? BuildPurpose(input.Scope),
            ["order_id"] = orderId,
            ["result_url"] = resultUrl,
            ["server_url"] = this.settings.ServerUrl,
            ["sandbox"] = this.settings.Sandbox ? 1 : 0,
        };

        // Для підписки — треба мінімум period або subscribe params.
        if (input.IsRecurring)
        {
            // Для MVP — місячна підписка
            payload["subscribe"] = "1";
            payload["subscribe_periodicity"] = "month";
        }

        // Дата старту у форматі yyyy-MM-dd HH:mm:ss
        var start = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        payload["subscribe_date_start"] = start;

        // Опціональні поля платника
        if (!string.IsNullOrWhiteSpace(input.PayerName))
        {
            payload["sender_first_name"] = input.PayerName;
        }

        if (!string.IsNullOrWhiteSpace(input.PayerPhone))
        {
            payload["sender_phone"] = input.PayerPhone;
        }

        if (!string.IsNullOrWhiteSpace(input.PayerEmail))
        {
            payload["sender_email"] = input.PayerEmail;
        }

        var json = JsonSerializer.Serialize(payload);
        var data = LiqPayCrypto.Base64(json);
        var signature = LiqPayCrypto.Sign(this.settings.PrivateKey, data);

        return new LiqPayCheckoutResponseDto(
            Data: data,
            Signature: signature,
            PublicKey: this.settings.PublicKey,
            GatewayUrl: "https://www.liqpay.ua/api/3/checkout",
            OrderId: orderId,
            ResultUrl: resultUrl);
    }

    /// <summary>
    /// Sends an asynchronous request to retrieve the status of a payment order by its identifier.
    /// </summary>
    /// <remarks>The returned JSON element includes status information as provided by the payment service.
    /// Ensure that the order identifier is valid and corresponds to an existing order. The method throws an exception
    /// if the HTTP request fails or the response cannot be deserialized.</remarks>
    /// <param name="orderId">The unique identifier of the payment order whose status is to be requested. Cannot be null or empty.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the status request operation.</param>
    /// <returns>A JSON element containing the response data for the requested order status. The structure of the JSON depends on
    /// the payment provider's API.</returns>
    public async Task<JsonElement> RequestStatusAsync(string orderId, CancellationToken ct = default)
    {
        var payload = new Dictionary<string, object?>
        {
            ["action"] = "status",
            ["version"] = 3,
            ["public_key"] = this.settings.PublicKey,
            ["order_id"] = orderId,
        };

        var json = JsonSerializer.Serialize(payload);
        var data = LiqPayCrypto.Base64(json);
        var signature = LiqPayCrypto.Sign(this.settings.PrivateKey, data);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["data"] = data,
            ["signature"] = signature,
        });

        var resp = await this.http.PostAsync("request", content, ct);
        resp.EnsureSuccessStatusCode();

        var doc = await resp.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        return doc;
    }

    private static string BuildPurpose(SubscriptionScope? scope) => scope switch
    {
        SubscriptionScope.Guardianship => "Опіка над твариною",
        SubscriptionScope.AidRequest => "Підтримка запиту на допомогу",
        _ => "Пожертва",
    };

    /// <summary>
    /// Builds a composite order identifier string that encodes subscription scope, entity, recurrence, user, anonymity,
    /// and a unique nonce.
    /// </summary>
    /// <remarks>The returned identifier is suitable for distinguishing orders based on scope, entity,
    /// recurrence, user, and anonymity. Each call generates a unique identifier due to the appended nonce.</remarks>
    /// <param name="scope">The subscription scope to include in the identifier. If null, 'Global' is used.</param>
    /// <param name="entityId">The unique identifier of the entity associated with the order. If null, a dash ('-') is used.</param>
    /// <param name="isRecurring">Indicates whether the order is recurring. Set to <see langword="true"/> for recurring orders; otherwise, <see
    /// langword="false"/>.</param>
    /// <param name="userId">The unique identifier of the user placing the order. If null, a dash ('-') is used.</param>
    /// <param name="anonymous">Indicates whether the order is placed anonymously. Set to <see langword="true"/> for anonymous orders;
    /// otherwise, <see langword="false"/>.</param>
    /// <returns>A string representing the composite order identifier, containing the encoded values of the provided parameters
    /// and a unique nonce.</returns>
    private static string BuildCompositeOrderId(
           SubscriptionScope? scope,
           Guid? entityId,
           bool isRecurring,
           Guid? userId,
           bool anonymous)
    {
        // "-" якщо немає значення
        string scopeStr = scope?.ToString() ?? "Global";
        string entityStr = entityId?.ToString("D") ?? "-";
        string recurringStr = isRecurring ? "1" : "0";
        string userStr = userId?.ToString("D") ?? "-";
        string anonStr = anonymous ? "1" : "0";
        string nonce = Guid.NewGuid().ToString("N");

        return $"{scopeStr}|{entityStr}|{recurringStr}|{userStr}|{anonStr}|{nonce}";
    }

    private string BuildRichResultUrl(CreateLiqPayCheckoutDto input, string orderId)
    {
        var q = System.Web.HttpUtility.ParseQueryString(string.Empty);
        q["status"] = "pending";
        q["orderId"] = orderId;
        q["isRecurring"] = input.IsRecurring ? "true" : "false";
        q["scope"] = input.Scope?.ToString();
        q["entityId"] = input.EntityId?.ToString();
        q["userId"] = input.UserId?.ToString();
        q["anonymous"] = input.Anonymous ? "true" : "false";
        q["amount"] = input.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture);
        q["currency"] = input.Currency;
        q["description"] = input.Description;
        q["payerName"] = input.PayerName;
        q["payerPhone"] = input.PayerPhone;
        q["payerEmail"] = input.PayerEmail;

        return $"{this.settings.ResultUrl}?{q}";
    }
}
