namespace PetCare.Domain.Entities;

using System;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a recurring payment subscription tracked by provider.
/// </summary>
public sealed class PaymentSubscription : BaseEntity
{
    private PaymentSubscription()
    {
    }

    private PaymentSubscription(
        Guid userId,
        Guid paymentMethodId,
        SubscriptionScope scopeType,
        Guid? scopeId,
        decimal amount,
        string currency,
        string provider,
        string providerSubscriptionId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Користувача не вказано.", nameof(userId));
        }

        if (paymentMethodId == Guid.Empty)
        {
            throw new ArgumentException("Метод оплати не вказано.", nameof(paymentMethodId));
        }

        if (amount <= 0)
        {
            throw new InvalidOperationException("Сума має бути більшою за 0.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new InvalidOperationException("Валюта не вказана.");
        }

        if (string.IsNullOrWhiteSpace(provider))
        {
            throw new InvalidOperationException("Провайдер не вказаний.");
        }

        if (string.IsNullOrWhiteSpace(providerSubscriptionId))
        {
            throw new InvalidOperationException("Ідентифікатор підписки провайдера не вказано.");
        }

        this.UserId = userId;
        this.PaymentMethodId = paymentMethodId;
        this.ScopeType = scopeType;
        this.ScopeId = scopeId;
        this.Amount = amount;
        this.Currency = currency;
        this.Provider = provider;
        this.ProviderSubscriptionId = providerSubscriptionId;

        this.Status = SubscriptionStatus.Active;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = this.CreatedAt;
    }

    /// <summary>Gets user who owns the subscription.</summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the current user associated with the context, or null if no user is set.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>Gets payment method used initially to create the subscription.</summary>
    public Guid PaymentMethodId { get; private set; }

    /// <summary>
    /// Gets the payment method associated with the current transaction, if one has been specified.
    /// </summary>
    public PaymentMethod? PaymentMethod { get; private set; }

    /// <summary>Gets logical scope for this subscription.</summary>
    public SubscriptionScope ScopeType { get; private set; }

    /// <summary>Gets optional id of scope entity (null for Global).</summary>
    public Guid? ScopeId { get; private set; }

    /// <summary>Gets recurring amount.</summary>
    public decimal Amount { get; private set; }

    /// <summary>Gets recurring currency (e.g., UAH).</summary>
    public string Currency { get; private set; } = "UAH";

    /// <summary>Gets payments provider (e.g., LiqPay).</summary>
    public string Provider { get; private set; } = default!;

    /// <summary>Gets provider subscription identifier.</summary>
    public string ProviderSubscriptionId { get; private set; } = default!;

    /// <summary>Gets lifecycle status.</summary>
    public SubscriptionStatus Status { get; private set; }

    /// <summary>Gets next scheduled charge moment (UTC), if known.</summary>
    public DateTime? NextChargeAt { get; private set; }

    /// <summary>Gets last successful charge moment (UTC), if any.</summary>
    public DateTime? LastChargeAt { get; private set; }

    /// <summary>Gets cancellation timestamp (UTC), if canceled.</summary>
    public DateTime? CanceledAt { get; private set; }

    /// <summary>Gets created timestamp (UTC).</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Gets updated timestamp (UTC).</summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new payment subscription for a user with the specified payment method, scope, amount, currency,
    /// provider, and provider subscription identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the payment subscription is being created.</param>
    /// <param name="paymentMethodId">The unique identifier of the payment method to be associated with the subscription.</param>
    /// <param name="scopeType">The scope type that defines the context or domain of the subscription, such as organization-wide or
    /// project-specific.</param>
    /// <param name="scopeId">The unique identifier of the scope within which the subscription applies. May be <see langword="null"/> if the
    /// scope is not specific.</param>
    /// <param name="amount">The monetary amount to be charged for the subscription. Must be a positive value.</param>
    /// <param name="currency">The ISO currency code representing the currency in which the subscription amount is denominated.</param>
    /// <param name="provider">The name of the payment provider responsible for processing the subscription.</param>
    /// <param name="providerSubscriptionId">The unique identifier assigned by the payment provider for the subscription.</param>
    /// <returns>A new <see cref="PaymentSubscription"/> instance initialized with the specified parameters.</returns>
    public static PaymentSubscription Create(
        Guid userId,
        Guid paymentMethodId,
        SubscriptionScope scopeType,
        Guid? scopeId,
        decimal amount,
        string currency,
        string provider,
        string providerSubscriptionId)
        => new(userId, paymentMethodId, scopeType, scopeId, amount, currency, provider, providerSubscriptionId);

    /// <summary>Set the next planned charge timestamp.</summary>
    /// <param name="whenUtc">The UTC timestamp for the next charge, or <see langword="null"/> if not scheduled.</param>
    public void SetNextCharge(DateTime? whenUtc)
    {
        this.NextChargeAt = whenUtc;
        this.Touch();
    }

    /// <summary>Mark subscription as charged at given time.</summary>
    /// <param name="whenUtc">The UTC timestamp when the charge occurred.</param>
    public void MarkCharged(DateTime whenUtc)
    {
        this.LastChargeAt = whenUtc;
        this.Touch();
    }

    /// <summary>Cancel this subscription.</summary>
    public void Cancel()
    {
        if (this.Status == SubscriptionStatus.Canceled)
        {
            return;
        }

        this.Status = SubscriptionStatus.Canceled;
        this.CanceledAt = DateTime.UtcNow;
        this.Touch();
    }

    /// <summary>Pause this subscription.</summary>
    public void Pause()
    {
        if (this.Status != SubscriptionStatus.Active)
        {
            throw new InvalidOperationException("Підписку можна призупинити лише у статусі Active.");
        }

        this.Status = SubscriptionStatus.Paused;
        this.Touch();
    }

    /// <summary>Resume a paused subscription.</summary>
    public void Resume()
    {
        if (this.Status != SubscriptionStatus.Paused)
        {
            throw new InvalidOperationException("Підписку можна відновити лише у статусі Paused.");
        }

        this.Status = SubscriptionStatus.Active;
        this.Touch();
    }

    private void Touch() => this.UpdatedAt = DateTime.UtcNow;
}
