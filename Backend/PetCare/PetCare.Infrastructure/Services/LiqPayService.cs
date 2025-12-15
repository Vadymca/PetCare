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
    private readonly IAnimalAidRequestService animalAidRequestService;
    private readonly ILogger<LiqPayService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiqPayService"/> class using the specified configuration settings and payment.
    /// service implementation.
    /// </summary>
    /// <param name="options">The configuration options containing LiqPay settings. Must not be null.</param>
    /// <param name="paymentService">The payment service implementation used to process payments. Must not be null.</param>
    /// <param name="paymentIntentService">The payment intent service implementation used to manage payment intents. Must not be null.</param>
    /// <param name="guardianshipRepository">The guardianship repository for accessing guardianship data. Must not be null.</param>
    /// <param name="animalAidRequestService">The animal aid request service for managing aid requests. Must not be null.</param>
    /// <param name="logger">The logger instance for logging information and errors. Must not be null.</param>
    public LiqPayService(
        IOptions<LiqPaySettings> options,
        IPaymentService paymentService,
        IPaymentIntentService paymentIntentService,
        IGuardianshipRepository guardianshipRepository,
        IAnimalAidRequestService animalAidRequestService,
        ILogger<LiqPayService> logger)
    {
        this.settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        this.paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        this.paymentIntentService = paymentIntentService ?? throw new ArgumentNullException(nameof(paymentIntentService));
        this.guardianshipRepository = guardianshipRepository ?? throw new ArgumentNullException(nameof(guardianshipRepository));
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
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
    public async Task<bool> ProcessCallbackAsync(
     string data,
     string signature,
     CancellationToken cancellationToken = default)
    {
        // 1. Validate LiqPay signature
        var expectedSignature = LiqPayCrypto.Sign(this.settings.PrivateKey, data);

        if (!string.Equals(expectedSignature, signature, StringComparison.Ordinal))
        {
            this.logger.LogWarning(
                "Invalid LiqPay signature. Expected {Expected}, got {Actual}",
                expectedSignature,
                signature);

            return false;
        }

        // 2. Decode and parse LiqPay body
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(data));
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        string? ReadString(JsonElement el) =>
            el.ValueKind switch
            {
                JsonValueKind.String => el.GetString(),
                JsonValueKind.Number => el.GetRawText(),
                _ => null,
            };

        var status = root.TryGetProperty("status", out var st) ? st.GetString() ?? "pending" : "pending";

        var liqpayOrderId = ReadString(root.GetProperty("order_id"));

        if (string.IsNullOrWhiteSpace(liqpayOrderId))
        {
            this.logger.LogError("Callback missing order_id. Raw JSON={Json}", json);
            return false;
        }

        this.logger.LogInformation("LiqPay callback for external order_id={LiqPayOrderId}", liqpayOrderId);

        // 3. Load internal payment intent (IMPORTANT FIX)
        var intent = await this.paymentIntentService.GetByExternalOrderIdAsync(liqpayOrderId, cancellationToken);

        if (intent is null)
        {
            this.logger.LogError("PaymentIntent not found for external order_id={OrderId}", liqpayOrderId);
            return false;
        }

        this.logger.LogInformation(
            "Resolved internal PaymentIntent: Id={IntentId}, OrderId={OrderId}, Scope={Scope}, ScopeId={ScopeId}",
            intent.Id,
            intent.ExternalOrderId,
            intent.ScopeType,
            intent.ScopeId);

        // 4. Extract payment info
        var targetEntity = intent.ScopeType?.ToString() ?? "Global";
        var targetEntityId = intent.ScopeId;
        bool isRecurring = intent.IsRecurring;
        Guid? userId = intent.UserId;
        bool anonymous = intent.Anonymous;

        // 5. Parse amount, currency, transaction ids
        var amount = root.TryGetProperty("amount", out var amEl) && amEl.ValueKind == JsonValueKind.Number
            ? amEl.GetDecimal()
            : 0m;

        var currency = root.TryGetProperty("currency", out var cc)
            ? cc.GetString() ?? "UAH"
            : "UAH";

        var transactionId =
            root.TryGetProperty("transaction_id", out var tx1) ? ReadString(tx1) :
            root.TryGetProperty("payment_id", out var tx2) ? ReadString(tx2) :
            intent.ExternalOrderId;

        // 6. Recurring specific fields
        DateTime? nextCharge = null;

        if (root.TryGetProperty("next_subscribe_date", out var nsd))
        {
            if (nsd.ValueKind == JsonValueKind.Number && nsd.TryGetInt64(out var ts))
            {
                nextCharge = DateTimeOffset.FromUnixTimeSeconds(ts).UtcDateTime;
            }

            if (nsd.ValueKind == JsonValueKind.String && long.TryParse(nsd.GetString(), out var ts2))
            {
                nextCharge = DateTimeOffset.FromUnixTimeSeconds(ts2).UtcDateTime;
            }
        }

        var providerSubscriptionId =
            root.TryGetProperty("subscribe_id", out var sid) ? ReadString(sid) : null;

        // 7. Normalize status
        if (status == "sandbox")
        {
            status = "success";
        }

        var isSuccess = status is "success" or "subscribed";
        var isFailure = status is "failure" or "error";

        // 8. SUCCESS
        if (isSuccess)
        {
            string? payerName = root.TryGetProperty("payer_name", out var pn) ? pn.GetString() : null;

            var donation = await this.paymentService.RecordChargeSuccessAsync(
                provider: "LiqPay",
                transactionId: transactionId!,
                amount: amount,
                currency: currency,
                targetEntity: targetEntity!,
                targetEntityId: targetEntityId,
                recurring: isRecurring,
                anonymous: anonymous,
                userId: userId,
                payerName: payerName,
                cancellationToken: cancellationToken);

            await this.paymentIntentService.AttachDonationAsync(
                intent.ExternalOrderId,
                donation.Id,
                cancellationToken);

            // FIX: Attach guardianship from intent, not from parsed
            if (intent.ScopeType == SubscriptionScope.Guardianship && intent.ScopeId is not null)
            {
                await this.paymentIntentService.AttachGuardianshipAsync(
                    intent.ExternalOrderId,
                    intent.ScopeId.Value,
                    cancellationToken);
            }

            if (intent.ScopeType == SubscriptionScope.AidRequest && intent.ScopeId is not null)
            {
                await this.animalAidRequestService.AttachDonationAsync(
                    intent.ScopeId.Value,
                    donation.Id,
                    cancellationToken);
            }

            // Recurring logic FIX
            if (isRecurring && userId.HasValue)
            {
                var providerSubscriptionIdOrLookup = transactionId;
                var subscription = await this.paymentService.FindSubscriptionByProviderIdAsync(providerSubscriptionIdOrLookup!, cancellationToken);

                if (subscription is not null)
                {
                    // Встановлюємо lastChargeAt як зараз
                    subscription.SetLastCharge(DateTime.UtcNow);

                    // Встановлюємо nextChargeAt
                    if (!nextCharge.HasValue)
                    {
                        nextCharge = DateTime.UtcNow.AddDays(30); // fallback 30 днів
                    }

                    subscription.SetNextCharge(nextCharge);

                    // Прив’язуємо підписку до PaymentIntent
                    await this.paymentIntentService.AttachSubscriptionAsync(intent.ExternalOrderId, subscription.Id, cancellationToken);
                }
            }

            return true;
        }

        // 9. FAILURE
        if (isFailure)
        {
            string? payerName =
                root.TryGetProperty("payer_name", out var pn) && !string.IsNullOrWhiteSpace(pn.GetString())
                    ? pn.GetString()
                    : intent.PayerName;

            await this.paymentIntentService.MarkFailedAsync(intent.ExternalOrderId, cancellationToken);

            await this.paymentService.RecordChargeFailedAsync(
                provider: "LiqPay",
                transactionId: transactionId,
                amount: amount,
                currency: currency,
                targetEntity: targetEntity!,
                targetEntityId: targetEntityId,
                recurring: isRecurring,
                anonymous: anonymous,
                userId: userId,
                payerName: payerName,
                cancellationToken: cancellationToken);

            return true;
        }

        // 10. NON-FINAL STATUS
        this.logger.LogInformation(
            "Ignoring non-final LiqPay status={Status} for order={Order}",
            status,
            intent.ExternalOrderId);

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
        if (parts.Length == 6)
        {
            var scopeStr = parts[0];
            var entityStr = parts[1];
            var recurringStr = parts[2];
            var userStr = parts[3];
            var anonStr = parts[4];

            Guid? entityId = entityStr == "-" ? null : TryParseGuid(entityStr);
            Guid? userId = userStr == "-" ? null : TryParseGuid(userStr);

            bool isRecurring = recurringStr == "1";
            bool anonymous = anonStr == "1";

            return (scopeStr, entityId, isRecurring, userId, anonymous);
        }

        // fallback: визначаємо scope за префіксом orderId
        string targetEntity;
        if (orderIdRaw.StartsWith("GRN-", StringComparison.OrdinalIgnoreCase))
        {
            targetEntity = "Guardianship";
        }
        else if (orderIdRaw.StartsWith("DON-", StringComparison.OrdinalIgnoreCase))
        {
            targetEntity = "Donation";
        }
        else if (orderIdRaw.StartsWith("SUB-", StringComparison.OrdinalIgnoreCase))
        {
            targetEntity = "Subscription";
        }
        else
        {
            targetEntity = "Global";
        }

        return (targetEntity, null, false, null, false);
    }

    private static Guid? TryParseGuid(string s)
    {
        if (Guid.TryParse(s?.Trim(), out var g))
        {
            return g;
        }

        return null;
    }
}
