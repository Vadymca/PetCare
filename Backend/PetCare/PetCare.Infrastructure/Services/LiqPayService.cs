namespace PetCare.Infrastructure.Services;

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetCare.Application.Interfaces;
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
    private readonly ILogger<LiqPayService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiqPayService"/> class using the specified configuration settings and payment.
    /// service implementation.
    /// </summary>
    /// <param name="options">The configuration options containing LiqPay settings. Must not be null.</param>
    /// <param name="paymentService">The payment service implementation used to process payments. Must not be null.</param>
    /// <param name="logger">The logger instance for logging information and errors. Must not be null.</param>
    public LiqPayService(
        IOptions<LiqPaySettings> options,
        IPaymentService paymentService,
        ILogger<LiqPayService> logger)
    {
        this.settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        this.paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

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

        // беремо всі поля м'яко
        var status = root.TryGetProperty("status", out var s) ? s.GetString() ?? "pending" : "pending";
        var action = root.TryGetProperty("action", out var a) ? a.GetString() : null;
        var orderIdRaw = root.TryGetProperty("order_id", out var o) ? o.GetString() : null;
        var amount = root.TryGetProperty("amount", out var am) ? am.GetDecimal() : 0m;
        var currency = root.TryGetProperty("currency", out var c) ? c.GetString() ?? "UAH" : "UAH";
        var transactionId = root.TryGetProperty("transaction_id", out var t) ? t.GetString()
                           : root.TryGetProperty("payment_id", out var p) ? p.GetString()
                           : orderIdRaw;

        // 3. Парсимо наш composite order_id
        if (string.IsNullOrWhiteSpace(orderIdRaw))
        {
            this.logger.LogError("LiqPay callback without order_id. Raw JSON: {Json}", json);
            return false;
        }

        var parsed = ParseCompositeOrderId(orderIdRaw);
        if (parsed is null)
        {
            this.logger.LogError("Failed to parse composite order_id: {OrderId}", orderIdRaw);
            return false;
        }

        var (targetEntity, targetEntityId, isRecurring, donorUserId, anonymous) = parsed.Value;

        // 4. В залежності від статусу — записуємо успіх або фейл
        if (status == "success")
        {
            this.logger.LogInformation(
                "Recording SUCCESS payment for {Target}({TargetId}) Tx={Tx} Amount={Amount}{Currency}",
                targetEntity,
                targetEntityId,
                transactionId,
                amount,
                currency);

            await this.paymentService.RecordChargeSuccessAsync(
                provider: "LiqPay",
                transactionId: transactionId!,
                amount: amount,
                currency: currency,
                targetEntity: targetEntity,
                targetEntityId: targetEntityId,
                recurring: isRecurring,
                anonymous: anonymous,
                userId: donorUserId,
                cancellationToken: cancellationToken);
        }
        else if (status == "failure" || status == "error")
        {
            this.logger.LogWarning(
                "Recording FAILED payment for {Target}({TargetId}) Tx={Tx} Amount={Amount}{Currency}",
                targetEntity,
                targetEntityId,
                transactionId,
                amount,
                currency);

            await this.paymentService.RecordChargeFailedAsync(
                provider: "LiqPay",
                transactionId: transactionId,
                amount: amount,
                currency: currency,
                targetEntity: targetEntity,
                targetEntityId: targetEntityId,
                recurring: isRecurring,
                anonymous: anonymous,
                userId: donorUserId,
                cancellationToken: cancellationToken);
        }
        else
        {
            // pending / wait_secure / etc. -> просто лог
            this.logger.LogInformation(
                "Received NON-FINAL LiqPay status '{Status}' for {Target}({TargetId}) Tx={Tx}",
                status,
                targetEntity,
                targetEntityId,
                transactionId);
        }

        return true;
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
        // orderId format:
        // {scope}|{entityId}|{isRecurring}|{userId}|{anonymous}|{nonceGuid}
        // приклад:
        // Guardianship|5c3b...|1|0123...|0|a36c5b...
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

        Guid? entityId = entityStr == "-" ? null : Guid.Parse(entityStr);
        Guid? userId = userStr == "-" ? null : Guid.Parse(userStr);

        bool isRecurring = recurringStr == "1";
        bool anonymous = anonStr == "1";

        // scopeStr напряму йде в Donation.TargetEntity
        // це дає нам "Guardianship", "AidRequest", "Global"
        return (scopeStr, entityId, isRecurring, userId, anonymous);
    }
}
