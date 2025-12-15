namespace PetCare.Application.Interfaces;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

/// <summary>
/// Defines high-level operations for managing <see cref="PaymentIntent"/> aggregates.
/// Application layer must use this service instead of accessing repositories directly.
/// </summary>
public interface IPaymentIntentService
{
    /// <summary>
    /// Creates a new LiqPay-based payment intent.
    /// </summary>
    /// <param name="scopeType">The scope type of the subscription, if applicable.</param>
    /// <param name="scopeId">The scope ID of the subscription, if applicable.</param>
    /// <param name="userId">The user ID associated with the payment intent, if applicable.</param>
    /// <param name="amount">The amount to be paid.</param>
    /// <param name="currency">The currency of the payment.</param>
    /// <param name="isRecurring">Indicates if the payment is recurring.</param>
    /// <param name="anonymous">Indicates if the payment is made anonymously.</param>
    /// <param name="payerName">The name of the payer, if available.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created <see cref="PaymentIntent"/>.</returns>
    Task<PaymentIntent> CreateLiqPayIntentAsync(
        SubscriptionScope? scopeType,
        Guid? scopeId,
        Guid? userId,
        decimal amount,
        string currency,
        bool isRecurring,
        bool anonymous,
        string? payerName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a payment intent by external order ID.
    /// </summary>
    /// <param name="externalOrderId">The external order ID.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="PaymentIntent"/> if found; otherwise, null.</returns>
    Task<PaymentIntent?> GetByExternalOrderIdAsync(
        string externalOrderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the payment intent as succeeded and sets provider payment identifier.
    /// </summary>
    /// <param name="externalOrderId">The external order ID.</param>
    /// <param name="providerPaymentId">The payment identifier from the payment provider.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task MarkSucceededAsync(
        string externalOrderId,
        string? providerPaymentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the payment intent as failed.
    /// </summary>
    /// <param name="externalOrderId">The external order ID.</param>
    /// <param name="cancellationToken" >A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task MarkFailedAsync(
        string externalOrderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Associates a donation with this payment intent.
    /// </summary>
    /// <param name="externalOrderId">The external order ID.</param>
    /// <param name="donationId">The donation ID to associate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AttachDonationAsync(
        string externalOrderId,
        Guid donationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Associates subscription with this payment intent.
    /// </summary>
    /// <param name="externalOrderId">The external order ID.</param>
    /// <param name="subscriptionId">The subscription ID to associate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AttachSubscriptionAsync(
        string externalOrderId,
        Guid subscriptionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Associates guardianship with this payment intent.
    /// </summary>
    /// <param name="externalOrderId">The external order ID.</param>
    /// <param name="guardianshipId">The guardianship ID to associate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AttachGuardianshipAsync(
        string externalOrderId,
        Guid guardianshipId,
        CancellationToken cancellationToken = default);
}
