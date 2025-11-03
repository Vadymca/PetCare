namespace PetCare.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Entities;

/// <summary>
/// Defines operations for managing recurring payment subscriptions.
/// </summary>
public interface ISubscriptionService
{
    /// <summary>
    /// Creates a new payment subscription for a specified guardianship and user with the given amount and currency.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the payment subscription is being created.</param>
    /// <param name="guardianshipId">The unique identifier of the guardianship associated with the payment subscription.</param>
    /// <param name="amount">The monetary amount to be charged for the subscription. Must be a positive value.</param>
    /// <param name="currency">The ISO 4217 currency code representing the currency in which the amount is specified. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created payment subscription.</returns>
    Task<PaymentSubscription> CreateForGuardianshipAsync(Guid userId, Guid guardianshipId, decimal amount, string currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels the guardianship process associated with the specified guardianship identifier asynchronously.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship to cancel.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous cancellation operation.</returns>
    Task CancelForGuardianshipAsync(Guid guardianshipId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new global payment subscription for the specified user with the given amount and currency.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the subscription is created. If null, the subscription is created
    /// without associating it to a specific user.</param>
    /// <param name="amount">The monetary amount for the subscription. Must be a positive value representing the subscription fee.</param>
    /// <param name="currency">The ISO 4217 currency code that specifies the currency of the subscription amount. For example, "USD" for US
    /// dollars.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created global payment
    /// subscription.</returns>
    Task<PaymentSubscription> CreateGlobalAsync(Guid? userId, decimal amount, string currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new payment subscription for the specified aid request and user with the given amount and currency.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the payment subscription is created. If null, the subscription may be
    /// created without associating a user.</param>
    /// <param name="aidRequestId">The unique identifier of the aid request to associate with the payment subscription.</param>
    /// <param name="amount">The monetary amount to be charged for the payment subscription. Must be a positive value.</param>
    /// <param name="currency">The ISO 4217 currency code representing the currency of the payment amount. For example, "USD" for US dollars.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created payment subscription
    /// associated with the aid request.</returns>
    Task<PaymentSubscription> CreateForAidRequestAsync(Guid? userId, Guid aidRequestId, decimal amount, string currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiates an asynchronous operation to cancel the specified provider subscription.
    /// </summary>
    /// <param name="providerSubscriptionId">The unique identifier of the provider subscription to cancel. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation. The default value is None.</param>
    /// <returns>A task that represents the asynchronous cancellation operation.</returns>
    Task CancelAsync(string providerSubscriptionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all active recurring payment subscriptions associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose recurring payment subscriptions are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="PaymentSubscription"/> objects representing the user's active recurring payment
    /// subscriptions. Returns an empty list if no subscriptions are found.</returns>
    Task<IReadOnlyList<PaymentSubscription>> GetMyRecurringAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the list of expected upcoming payments for the specified user, including each payment
    /// subscription and its next scheduled charge date.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose expected payments are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of tuples, each containing a payment subscription and the next scheduled charge date. The
    /// charge date is <see langword="null"/> if no future charge is scheduled.</returns>
    Task<IReadOnlyList<(PaymentSubscription Subscription, DateTime? NextChargeAt)>> GetMyExpectedPaymentsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a payment subscription that matches the specified provider subscription identifier.
    /// </summary>
    /// <param name="providerSubscriptionId">The unique identifier assigned to the subscription by the external payment provider. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching PaymentSubscription if
    /// found; otherwise, null.</returns>
    Task<PaymentSubscription?> FindByProviderSubscriptionIdAsync(
        string providerSubscriptionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels all subscriptions that have expired or are no longer valid.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The number of subscriptions that were canceled.</returns>
    Task<int> CancelExpiredAsync(CancellationToken cancellationToken = default);
}
