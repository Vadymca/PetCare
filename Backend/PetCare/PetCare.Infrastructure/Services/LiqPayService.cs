namespace PetCare.Infrastructure.Services;

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Enums;
using PetCare.Infrastructure.Options;
using PetCare.Infrastructure.Payments;

/// <summary>
/// Provides LiqPay payment callback processing and integration with the application's payment service.
/// </summary>
/// <remarks>This service validates LiqPay callback signatures and updates payment records based on the callback
/// status. It is intended to be used as part of the payment workflow for handling asynchronous notifications from
/// LiqPay. The class is sealed and should be accessed via the ILiqPayService interface. Thread safety is ensured for
/// typical usage scenarios.</remarks>
public sealed class LiqPayService : ILiqPayService
{
    private readonly LiqPaySettings settings;
    private readonly IPaymentService paymentService;
    private readonly IPaymentIntentService paymentIntentService;
    private readonly IGuardianshipRepository guardianshipRepository;
    private readonly ILogger<LiqPayService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiqPayService"/> class using the specified configuration settings and payment.
    /// service implementation.
    /// </summary>
    /// <param name="options">The configuration options containing LiqPay settings. Must not be null.</param>
    /// <param name="paymentService">The payment service implementation used to process payments. Must not be null.</param>
    /// <param name="paymentIntentService">The payment intent service implementation used to manage payment intents. Must not be null.</param>
    /// <param name="guardianshipRepository">The guardianship repository for accessing guardianship data. Must not be null.</param>
    /// <param name="logger">The logger instance for logging information and errors. Must not be null.</param>
    public LiqPayService(
        IOptions<LiqPaySettings> options,
        IPaymentService paymentService,
        IPaymentIntentService paymentIntentService,
        IGuardianshipRepository guardianshipRepository,
        ILogger<LiqPayService> logger)
    {
        this.settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        this.paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        this.paymentIntentService = paymentIntentService ?? throw new ArgumentNullException(nameof(paymentIntentService));
        this.guardianshipRepository = guardianshipRepository ?? throw new ArgumentNullException(nameof(guardianshipRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private bool IsSandbox =>
        this.settings.PublicKey.StartsWith("sandbox_", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Processes a LiqPay payment callback by verifying the signature and recording the payment result asynchronously.
    /// </summary>
    /// <remarks>This method verifies the callback's signature before processing the payment result. If the
    /// signature is invalid, the method returns false and no payment record is updated. If the signature is valid, the
    /// payment status is recorded as either successful or failed based on the callback data. The method does not throw
    /// exceptions for invalid signatures; callers should check the return value to determine if processing was
    /// successful.</remarks>
    /// <param name="data">The base64-encoded JSON string containing the callback data from LiqPay.</param>
    /// <param name="signature">The signature string used to verify the authenticity of the callback data.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>true if the callback signature is valid and the payment result was processed; otherwise, false.</returns>
    public async Task<bool> ProcessCallbackAsync(string data, string signature, CancellationToken cancellationToken = default)
    {
        // 1. Перевірка сигнатури
        var expected = LiqPayCrypto.Sign(this.settings.PrivateKey, data);
        if (!string.Equals(expected, signature, StringComparison.Ordinal))
        {
            this.logger.LogWarning(
                "Invalid LiqPay signature. Expected {Expected}, got {Actual}",
                expected,
                signature);
            return false;
        }

        // 2. Розпаковуємо body
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(data));
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Helper: читає string або number як string
        static string? ReadFlexibleString(JsonElement el)
        {
            return el.ValueKind switch
            {
                JsonValueKind.String => el.GetString(),
                JsonValueKind.Number => el.GetRawText(),
                _ => null,
            };
        }

        // беремо всі поля м'яко
        var status = root.TryGetProperty("status", out var s) ? s.GetString() ?? "pending" : "pending";
        var action = root.TryGetProperty("action", out var a) ? a.GetString() : null;

        var orderId = root.TryGetProperty("order_id", out var o)
            ? ReadFlexibleString(o)
            : null;

        if (string.IsNullOrWhiteSpace(orderId))
        {
            this.logger.LogError("Callback missing order_id. Raw JSON: {Json}", json);
            return false;
        }

        this.logger.LogInformation("Callback received for order_id: {Id}", orderId);

        var amount = root.TryGetProperty("amount", out var am) && am.ValueKind is JsonValueKind.Number
            ? am.GetDecimal()
            : 0m;

        var currency = root.TryGetProperty("currency", out var c)
            ? c.GetString() ?? "UAH"
            : "UAH";

        // transaction_id може бути number
        var transactionId =
            root.TryGetProperty("transaction_id", out var t) ? ReadFlexibleString(t) :
            root.TryGetProperty("payment_id", out var p) ? ReadFlexibleString(p) :
            orderId;

        // читаємо next_subscribe_date ----
        DateTime? nextChargeUtc = null;

        if (root.TryGetProperty("next_subscribe_date", out var nsd))
        {
            if (nsd.ValueKind == JsonValueKind.Number && nsd.TryGetInt64(out var unix))
            {
                nextChargeUtc = DateTimeOffset.FromUnixTimeSeconds(unix).UtcDateTime;
            }
            else if (nsd.ValueKind == JsonValueKind.String && long.TryParse(nsd.GetString(), out var unixStr))
            {
                nextChargeUtc = DateTimeOffset.FromUnixTimeSeconds(unixStr).UtcDateTime;
            }
        }

        // читаємо subscribe_id (ід підписки LiqPay)
        string? providerSubscriptionId = null;

        if (root.TryGetProperty("subscribe_id", out var sid))
        {
            providerSubscriptionId = ReadFlexibleString(sid);
        }

        // 3. Парсимо наш composite order_id
        var parsed = ParseCompositeOrderId(orderId);

        string targetEntity;
        Guid? targetEntityId;
        bool isRecurring;
        Guid? userId;
        bool anonymous;

        if (parsed is null)
        {
            // PARSE FAILED — fallback logic:
            // - In sandbox: treat as a one-shot Global (but continue processing) — generate useful providerSubscriptionId if missing.
            // - In prod: treat as Global as well (but log warnings).
            this.logger.LogWarning("Failed to parse composite order_id: {OrderId}. Using fallback.", orderId);

            // fallback values
            targetEntity = "Global";
            targetEntityId = null;
            isRecurring = false;
            userId = null;
            anonymous = false;

            // ensure providerSubscriptionId exists in sandbox for recurring flows that expect it
            if (this.IsSandbox && string.IsNullOrWhiteSpace(providerSubscriptionId))
            {
                providerSubscriptionId = $"sandbox-fallback-{Guid.NewGuid()}";
                this.logger.LogDebug("Sandbox fallback providerSubscriptionId generated: {Id}", providerSubscriptionId);
            }
        }
        else
        {
            (targetEntity, targetEntityId, isRecurring, userId, anonymous) = parsed.Value;
        }

        // 4. У нас sandbox завжди = success
        if (status == "sandbox")
        {
            status = "success";
            this.logger.LogInformation(
                "Sandbox treated as success for {Id}", orderId);
        }

        // Реально успішні статуси
        bool isSuccess = status is "success" or "subscribed";

        // Помилкові статуси
        bool isFailure = status is "failure" or "error";

        // 5. Обробка успіху
        if (isSuccess)
        {
            this.logger.LogInformation("SUCCESS payment, Scope={Scope} ScopeId={ScopeId}, Tx={Tx}", targetEntity, targetEntityId, transactionId);

            // 1. Реєструємо факт успіху (створює Donation/Subscription/оновлює Guardianship)
            var donation = await this.paymentService.RecordChargeSuccessAsync(
               provider: "LiqPay",
               transactionId: transactionId!,
               amount: amount,
               currency: currency,
               targetEntity: targetEntity,
               targetEntityId: targetEntityId,
               recurring: isRecurring,
               anonymous: anonymous,
               userId: userId,
               cancellationToken: cancellationToken);

            // 2. Прив’язуємо сутності до PaymentIntent
            // 2️⃣ Attach Donation to Intent
            await this.paymentIntentService.AttachDonationAsync(
                orderId,
                donation.Id,
                cancellationToken);

            if (targetEntity == "Guardianship" && targetEntityId is not null)
            {
                await this.paymentIntentService.AttachGuardianshipAsync(
                    orderId,
                    targetEntityId.Value,
                    cancellationToken);
            }

            if (isRecurring)
            {
                // If providerSubscriptionId missing — attempt to fallback to orderId (sandbox cases)
                var providerIdToSearch = !string.IsNullOrWhiteSpace(providerSubscriptionId) ? providerSubscriptionId : orderId;

                var subscription = await this.paymentService
                    .FindSubscriptionByProviderIdAsync(providerIdToSearch, cancellationToken);

                if (subscription is not null)
                {
                    subscription.MarkCharged(DateTime.UtcNow);
                    subscription.SetNextCharge(nextChargeUtc);

                    await this.paymentService.UpdateSubscriptionAsync(subscription, cancellationToken);

                    this.logger.LogInformation(
                        "Updated subscription {Id}: LastChargeAt={Last}, NextChargeAt={Next}",
                        subscription.Id,
                        subscription.LastChargeAt,
                        subscription.NextChargeAt);
                }
                else
                {
                    this.logger.LogWarning(
                        "Recurring payment received but subscription not found. ProviderSubscriptionId={Id}",
                        providerSubscriptionId);
                }
            }

            return true;
        }

        // 6. Обробка помилки
        if (isFailure)
        {
            this.logger.LogWarning(
               "FAILED payment. Scope={Scope} ScopeId={ScopeId} Tx={Tx}",
               targetEntity,
               targetEntityId,
               transactionId);

            await this.paymentIntentService.MarkFailedAsync(
                orderId,
                cancellationToken);

            // record failed donation
            await this.paymentService.RecordChargeFailedAsync(
                provider: "LiqPay",
                transactionId: transactionId,
                amount: amount,
                currency: currency,
                targetEntity: targetEntity,
                targetEntityId: targetEntityId,
                recurring: isRecurring,
                anonymous: anonymous,
                userId: userId,
                cancellationToken: cancellationToken);

            return true;
        }

        // 7. Не фінальний статус — просто лог
        this.logger.LogInformation(
            "Non-final LiqPay status '{Status}' for order {OrderId}",
            status,
            orderId);

        return true;
    }

    /// <summary>
    /// Creates a new recurring payment contract for the specified user using the LiqPay payment provider.
    /// </summary>
    /// <remarks>This method initiates a recurring payment contract with LiqPay and returns information
    /// necessary to manage the subscription. The contract is created immediately upon successful completion of the
    /// operation. Ensure that the user has a valid payment method associated with the LiqPay provider before calling
    /// this method.</remarks>
    /// <param name="userId">The unique identifier of the user for whom the recurring contract is being created.</param>
    /// <param name="amount">The amount to be charged for each recurring payment. Must be a positive value.</param>
    /// <param name="currency">The three-letter ISO currency code (e.g., "USD", "UAH") in which the recurring payment will be charged.</param>
    /// <param name="scope">The subscription scope that defines the context or type of the recurring contract.</param>
    /// <param name="scopeId">The unique identifier of the specific scope instance for the subscription. Can be null if not applicable.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a LiqPayRecurringResponseDto with
    /// details of the created recurring contract, including the provider subscription ID, payment method ID, and the
    /// date of the next scheduled charge if available.</returns>
    public async Task<LiqPayRecurringResponseDto> CreateRecurringContractAsync(
     Guid userId,
     decimal amount,
     string currency,
     SubscriptionScope scope,
     Guid? scopeId,
     CancellationToken cancellationToken = default)
    {
        // 1. Формуємо order_id в тому ж форматі, що і checkout
        var orderId = $"{scope}|{scopeId?.ToString() ?? "-"}|1|{userId}|0|{Guid.NewGuid()}";

        // 2. Формуємо body для LiqPay
        var requestBody = new
        {
            action = "subscribe",
            version = 3,
            amount = amount,
            currency = currency,
            description = "Повторна підписка",
            order_id = orderId,
            public_key = this.settings.PublicKey,
        };

        var json = JsonSerializer.Serialize(requestBody);
        var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        var signature = LiqPayCrypto.Sign(this.settings.PrivateKey, data);

        var form = new Dictionary<string, string>
    {
        { "data", data },
        { "signature", signature },
    };

        using var http = new HttpClient();
        var httpResponse = await http.PostAsync(
            "https://www.liqpay.ua/api/request",
            new FormUrlEncodedContent(form),
            cancellationToken);

        var respJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(respJson);

        var root = doc.RootElement;

        string? providerSubId = null;
        if (root.TryGetProperty("subscribe_id", out var subIdProp))
        {
            providerSubId = subIdProp.GetString();
        }

        long? nextUnix = null;
        if (root.TryGetProperty("next_subscribe_date", out var nextProp) &&
            nextProp.TryGetInt64(out var unix))
        {
            nextUnix = unix;
        }

        // ================= SANDBOX FALLBACK =================
        if (this.IsSandbox)
        {
            if (string.IsNullOrWhiteSpace(providerSubId))
            {
                providerSubId = $"sandbox_sub_{Guid.NewGuid()}";
            }

            if (nextUnix == null)
            {
                nextUnix = DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds();
            }
        }

        // ====================================================
        DateTime? nextChargeAt = nextUnix.HasValue
            ? DateTimeOffset.FromUnixTimeSeconds(nextUnix.Value).UtcDateTime
            : null;

        // 3. Витягуємо PaymentMethodId з нашої БД
        var paymentMethodId = await this.guardianshipRepository
            .RequirePaymentMethodIdByProviderAsync("LiqPay", cancellationToken);

        return new LiqPayRecurringResponseDto(
            providerSubId!,
            paymentMethodId,
            nextChargeAt);
    }

    /// <summary>
    /// Parses a composite order identifier string and extracts its constituent components, including target entity,
    /// entity ID, recurrence status, user ID, and anonymity flag.
    /// </summary>
    /// <remarks>The method expects the input string to contain exactly six components separated by pipe
    /// characters. If the format is invalid, the method returns null. The target entity and user ID components may be
    /// null if represented by a dash ('-') in the input.</remarks>
    /// <param name="orderIdRaw">The raw order identifier string to parse. Must be in the format
    /// '{scope}|{entityId}|{isRecurring}|{userId}|{anonymous}|{nonceGuid}', where each component is separated by a pipe
    /// ('|') character.</param>
    /// <returns>A tuple containing the target entity name, target entity ID, recurrence status, user ID, and anonymity flag if
    /// parsing succeeds; otherwise, null if the input does not match the expected format.</returns>
    private static (string TargetEntity, Guid? TargetEntityId, bool IsRecurring, Guid? UserId, bool Anonymous)?
             ParseCompositeOrderId(string orderIdRaw)
    {
        if (string.IsNullOrWhiteSpace(orderIdRaw))
        {
            return null;
        }

        var parts = orderIdRaw.Split('|');
        if (parts.Length != 6)
        {
            return null;
        }

        var scopeStr = parts[0]; // "Global" / "AidRequest" / "Guardianship"
        var entityStr = parts[1]; // "-" або Guid
        var recurringStr = parts[2]; // "0"/"1"
        var userStr = parts[3]; // "-" або Guid
        var anonStr = parts[4]; // "0"/"1"

        Guid? entityId = entityStr == "-" ? null : TryParseGuid(entityStr);
        Guid? userId = userStr == "-" ? null : TryParseGuid(userStr);

        bool isRecurring = recurringStr == "1";
        bool anonymous = anonStr == "1";

        // scopeStr напряму йде в Donation.TargetEntity
        // це дає нам "Guardianship", "AidRequest", "Global"
        return (scopeStr, entityId, isRecurring, userId, anonymous);
    }

    private static Guid? TryParseGuid(string s)
    {
        if (Guid.TryParse(s, out var g))
        {
            return g;
        }

        return null;
    }
}
