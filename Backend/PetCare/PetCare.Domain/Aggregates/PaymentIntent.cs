namespace PetCare.Domain.Aggregates;

using System;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a high-level description of an upcoming or completed payment,
/// which ties together the payment provider, logical scope, and resulting domain entities
/// such as donations, subscriptions, or guardianships.
/// </summary>
public sealed class PaymentIntent : BaseEntity
{
    private PaymentIntent()
    {
    }

    private PaymentIntent(
        string externalOrderId,
        string paymentProvider,
        SubscriptionScope? scopeType,
        Guid? scopeId,
        Guid? userId,
        decimal amount,
        string currency,
        bool isRecurring,
        bool anonymous)
    {
        if (string.IsNullOrWhiteSpace(externalOrderId))
        {
            throw new InvalidOperationException("Зовнішній ідентифікатор платежу (ExternalOrderId) не може бути порожнім.");
        }

        if (externalOrderId.Length > 64)
        {
            throw new InvalidOperationException("Зовнішній ідентифікатор платежу (ExternalOrderId) не може перевищувати 64 символи.");
        }

        if (string.IsNullOrWhiteSpace(paymentProvider))
        {
            throw new InvalidOperationException("Платіжний провайдер не вказаний.");
        }

        if (amount <= 0)
        {
            throw new InvalidOperationException("Сума має бути більшою за 0.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new InvalidOperationException("Валюта не вказана.");
        }

        this.ExternalOrderId = externalOrderId;
        this.PaymentProvider = paymentProvider;
        this.ScopeType = scopeType;
        this.ScopeId = scopeId;
        this.UserId = userId;
        this.Amount = amount;
        this.Currency = currency;
        this.IsRecurring = isRecurring;
        this.Anonymous = anonymous;

        this.Status = PaymentIntentStatus.Pending;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = this.CreatedAt;
    }

    /// <summary>
    /// Gets the external order identifier used by the payment provider (e.g., LiqPay order_id).
    /// Must be unique and not exceed 64 characters.
    /// </summary>
    public string ExternalOrderId { get; private set; } = default!;

    /// <summary>
    /// Gets the name of the payment provider (e.g., "LiqPay").
    /// </summary>
    public string PaymentProvider { get; private set; } = default!;

    /// <summary>
    /// Gets the provider-specific payment identifier (e.g., LiqPay payment_id), if available.
    /// </summary>
    public string? ProviderPaymentId { get; private set; }

    /// <summary>
    /// Gets the logical scope of this payment (e.g., Guardianship, AidRequest, Global).
    /// </summary>
    public SubscriptionScope? ScopeType { get; private set; }

    /// <summary>
    /// Gets the identifier of the scoped entity (e.g., guardianship, project, animal), if applicable.
    /// </summary>
    public Guid? ScopeId { get; private set; }

    /// <summary>
    /// Gets the user associated with this intent, if known.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets the user navigation property.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the total amount for this payment intent.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Gets the currency of this payment (e.g., "UAH").
    /// </summary>
    public string Currency { get; private set; } = "UAH";

    /// <summary>
    /// Gets a value indicating whether this payment intent represents a recurring payment.
    /// </summary>
    public bool IsRecurring { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the payer chose to stay anonymous.
    /// </summary>
    public bool Anonymous { get; private set; }

    /// <summary>
    /// Gets the current lifecycle status of this payment intent.
    /// </summary>
    public PaymentIntentStatus Status { get; private set; }

    /// <summary>
    /// Gets the timestamp (UTC) when the intent was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the timestamp (UTC) when the intent was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the identifier of the resulting donation, if such was created.
    /// </summary>
    public Guid? DonationId { get; private set; }

    /// <summary>
    /// Gets the donation created as a result of this payment, if any.
    /// </summary>
    public Donation? Donation { get; private set; }

    /// <summary>
    /// Gets the identifier of the resulting subscription, if any.
    /// </summary>
    public Guid? SubscriptionId { get; private set; }

    /// <summary>
    /// Gets the payment subscription created as a result of this payment, if any.
    /// </summary>
    public PaymentSubscription? Subscription { get; private set; }

    /// <summary>
    /// Gets the identifier of the guardianship associated with this payment intent, if any.
    /// </summary>
    public Guid? GuardianshipId { get; private set; }

    /// <summary>
    /// Gets the guardianship associated with this payment intent, if any.
    /// </summary>
    public Guardianship? Guardianship { get; private set; }

    /// <summary>
    /// Factory method to create a new payment intent for a LiqPay-based payment flow.
    /// </summary>
    /// <param name="scopeType">Logical scope of the payment (e.g., guardianship, aid request, global).</param>
    /// <param name="scopeId">Identifier of the scoped entity, if applicable.</param>
    /// <param name="userId">User initiating the payment, if available.</param>
    /// <param name="amount">Payment amount. Must be greater than zero.</param>
    /// <param name="currency">Currency code (e.g., "UAH").</param>
    /// <param name="isRecurring">Indicates whether the payment is recurring.</param>
    /// <param name="anonymous">Indicates whether the payer chose to stay anonymous.</param>
    /// <returns>A new <see cref="PaymentIntent"/> instance.</returns>
    public static PaymentIntent CreateForLiqPay(
        SubscriptionScope? scopeType,
        Guid? scopeId,
        Guid? userId,
        decimal amount,
        string currency,
        bool isRecurring,
        bool anonymous)
    {
        string externalOrderId = BuildExternalOrderId(scopeType, scopeId, userId, isRecurring, anonymous);

        return new PaymentIntent(
            externalOrderId,
            paymentProvider: "LiqPay",
            scopeType,
            scopeId,
            userId,
            amount,
            currency,
            isRecurring,
            anonymous);
    }

    /// <summary>
    /// Marks this payment intent as successfully completed and stores the provider payment identifier.
    /// </summary>
    /// <param name="providerPaymentId">The provider payment identifier (e.g., LiqPay payment_id), if available.</param>
    public void MarkSucceeded(string? providerPaymentId)
    {
        this.Status = PaymentIntentStatus.Succeeded;

        if (!string.IsNullOrWhiteSpace(providerPaymentId))
        {
            this.ProviderPaymentId = providerPaymentId;
        }

        this.Touch();
    }

    /// <summary>
    /// Marks this payment intent as failed.
    /// </summary>
    public void MarkFailed()
    {
        this.Status = PaymentIntentStatus.Failed;
        this.Touch();
    }

    /// <summary>
    /// Marks this payment intent as canceled.
    /// </summary>
    public void MarkCanceled()
    {
        this.Status = PaymentIntentStatus.Canceled;
        this.Touch();
    }

    /// <summary>
    /// Attaches a donation created as a result of this payment intent.
    /// </summary>
    /// <param name="donationId">The donation identifier.</param>
    public void AttachDonation(Guid donationId)
    {
        if (donationId == Guid.Empty)
        {
            throw new InvalidOperationException("Ідентифікатор донату не може бути порожнім.");
        }

        this.DonationId = donationId;
        this.Touch();
    }

    /// <summary>
    /// Attaches a subscription created as a result of this payment intent.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    public void AttachSubscription(Guid subscriptionId)
    {
        if (subscriptionId == Guid.Empty)
        {
            throw new InvalidOperationException("Ідентифікатор підписки не може бути порожнім.");
        }

        this.SubscriptionId = subscriptionId;
        this.Touch();
    }

    /// <summary>
    /// Attaches a guardianship associated with this payment intent.
    /// </summary>
    /// <param name="guardianshipId">The guardianship identifier.</param>
    public void AttachGuardianship(Guid guardianshipId)
    {
        if (guardianshipId == Guid.Empty)
        {
            throw new InvalidOperationException("Ідентифікатор опіки не може бути порожнім.");
        }

        this.GuardianshipId = guardianshipId;
        this.Touch();
    }

    private static string BuildExternalOrderId(
        SubscriptionScope? scopeType,
        Guid? scopeId,
        Guid? userId,
        bool isRecurring,
        bool anonymous)
    {
        // Single-character scope code: G (Global), G (Guardianship), A (AidRequest) etc.
        var scopeCode = scopeType?.ToString()[0].ToString() ?? "G";
        var recurringCode = isRecurring ? "R" : "O";
        var anonCode = anonymous ? "A" : "N";

        var entityPart = scopeId?.ToString("N")[..8] ?? "00000000";
        var userPart = userId?.ToString("N")[..8] ?? "00000000";
        var nonce = Guid.NewGuid().ToString("N")[..8];

        // Example: GRA-abcdef12-12345678-90ab12cd  (length ~ 3 + 1 + 8 + 1 + 8 + 1 + 8 <= 64)
        return $"{scopeCode}{recurringCode}{anonCode}-{entityPart}-{userPart}-{nonce}";
    }

    private void Touch()
    {
        this.UpdatedAt = DateTime.UtcNow;
    }
}
