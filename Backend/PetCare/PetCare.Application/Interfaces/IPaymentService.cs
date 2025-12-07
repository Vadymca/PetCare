namespace PetCare.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Defines operations for recording payments and building payment return payloads.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Records a successful payment transaction and creates a corresponding donation entry.
    /// </summary>
    /// <param name="provider">The name of the payment provider that processed the transaction. Cannot be null or empty.</param>
    /// <param name="transactionId">The unique identifier of the transaction as provided by the payment provider. Cannot be null or empty.</param>
    /// <param name="amount">The amount of the donation in the specified currency. Must be a positive value.</param>
    /// <param name="currency">The ISO currency code representing the currency of the donation. Cannot be null or empty.</param>
    /// <param name="targetEntity">The type or name of the entity that is the target of the donation (e.g., campaign, project). Cannot be null or
    /// empty.</param>
    /// <param name="targetEntityId">The unique identifier of the target entity receiving the donation, if applicable.</param>
    /// <param name="recurring">Indicates whether the donation is part of a recurring payment schedule. Set to <see langword="true"/> for
    /// recurring donations; otherwise, <see langword="false"/>.</param>
    /// <param name="anonymous">Indicates whether the donation should be recorded as anonymous. Set to <see langword="true"/> to hide donor
    /// identity; otherwise, <see langword="false"/>.</param>
    /// <param name="userId">The unique identifier of the user making the donation, if available.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="Donation"/>
    /// object representing the recorded donation.</returns>
    Task<Donation> RecordChargeSuccessAsync(
        string provider,
        string transactionId,
        decimal amount,
        string currency,
        string targetEntity,
        Guid? targetEntityId,
        bool recurring,
        bool anonymous,
        Guid? userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Records a failed donation charge attempt and creates a corresponding donation entry with failure details.
    /// </summary>
    /// <param name="provider">The name of the payment provider that processed the charge attempt. Cannot be null or empty.</param>
    /// <param name="transactionId">The unique identifier of the transaction from the payment provider, if available. May be null if not provided by
    /// the provider.</param>
    /// <param name="amount">The monetary amount of the attempted donation. Must be a non-negative value.</param>
    /// <param name="currency">The ISO currency code representing the currency of the donation (e.g., "USD"). Cannot be null or empty.</param>
    /// <param name="targetEntity">The type or name of the entity that the donation was intended for (such as a campaign or organization). Cannot
    /// be null or empty.</param>
    /// <param name="targetEntityId">The unique identifier of the target entity, if available. May be null if not applicable.</param>
    /// <param name="recurring">Indicates whether the donation was intended to be a recurring payment. Set to <see langword="true"/> for
    /// recurring donations; otherwise, <see langword="false"/>.</param>
    /// <param name="anonymous">Indicates whether the donor chose to remain anonymous. Set to <see langword="true"/> if the donation is
    /// anonymous; otherwise, <see langword="false"/>.</param>
    /// <param name="userId">The unique identifier of the user who attempted the donation, if available. May be null for anonymous donations
    /// or if the user is not registered.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the donation entry reflecting the
    /// failed charge attempt.</returns>
    Task<Donation> RecordChargeFailedAsync(
        string provider,
        string? transactionId,
        decimal amount,
        string currency,
        string targetEntity,
        Guid? targetEntityId,
        bool recurring,
        bool anonymous,
        Guid? userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Builds a URL for retrieving the payment status, including the specified status and additional query parameters.
    /// </summary>
    /// <param name="basePath">The base path of the URL to which the payment status and query parameters will be appended. Must be a valid URL
    /// or URI segment.</param>
    /// <param name="status">The payment status to include in the URL, such as "pending", "completed", or "failed". This value is typically
    /// used to indicate the desired status to query.</param>
    /// <param name="data">A collection of key-value pairs representing additional query parameters to include in the URL. Keys must be
    /// non-null; values may be null to indicate an empty parameter value.</param>
    /// <returns>A string containing the constructed URL with the payment status and any additional query parameters appended.
    /// The returned URL is suitable for use in HTTP requests.</returns>
    string BuildPaymentStatusUrl(string basePath, string status, IDictionary<string, string?> data);

    /// <summary>
    /// Asynchronously retrieves a read-only list of donations made by the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose payments are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of <see
    /// cref="Donation"/> objects associated with the specified user. If the user has not made any payments, the list
    /// will be empty.</returns>
    Task<IReadOnlyList<Donation>> GetMyPaymentsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all donations ordered by most recent first.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="Donation"/>.</returns>
    Task<IReadOnlyList<Donation>> ListAllDonationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves donations filtered by a specific project.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="Donation"/>.</returns>
    Task<IReadOnlyList<Donation>> ListDonationsByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a recurring subscription by its provider subscription identifier
    /// (e.g., LiqPay's subscription_id).
    /// </summary>
    /// <param name="providerSubscriptionId">The subscription identifier assigned by the payment provider.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="PaymentSubscription"/> instance if found; otherwise <see langword="null"/>.
    /// </returns>
    Task<PaymentSubscription?> FindSubscriptionByProviderIdAsync(
        string providerSubscriptionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists changes made to the specified <see cref="PaymentSubscription"/> instance.
    /// </summary>
    /// <param name="subscription">The subscription to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateSubscriptionAsync(
        PaymentSubscription subscription,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets a user's payment subscription by creating a new subscription with the specified parameters and
    /// deactivating the previous one.
    /// </summary>
    /// <remarks>The previous subscription specified by oldSubscriptionId will be deactivated as part of this
    /// operation. The new subscription will be created with the provided parameters. This method does not modify the
    /// payment method or provider details beyond associating them with the new subscription.</remarks>
    /// <param name="oldSubscriptionId">The unique identifier of the existing subscription to be replaced. The subscription with this ID will be
    /// deactivated.</param>
    /// <param name="userId">The unique identifier of the user for whom the subscription is being reset.</param>
    /// <param name="amount">The recurring payment amount for the new subscription. Must be a non-negative value.</param>
    /// <param name="currency">The ISO 4217 currency code for the subscription payment (for example, "USD"). Cannot be null or empty.</param>
    /// <param name="scope">The scope of the subscription, indicating the context or level at which the subscription applies.</param>
    /// <param name="scopeId">The unique identifier for the scope, if applicable. May be null if the scope does not require an identifier.</param>
    /// <param name="provider">The name of the payment provider to be used for the new subscription. Cannot be null or empty.</param>
    /// <param name="paymentMethodId">The unique identifier of the payment method to be associated with the new subscription.</param>
    /// <param name="providerSubscriptionId">The identifier of the new subscription as assigned by the payment provider. Cannot be null or empty.</param>
    /// <param name="nextChargeAt">The date and time when the next charge should occur for the new subscription, or null to use the default
    /// schedule.</param>
    /// <param name="externalOrderId">An optional external order identifier associated with the subscription.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created payment
    /// subscription.</returns>
    Task<PaymentSubscription> ResetSubscriptionAsync(
        Guid oldSubscriptionId,
        Guid userId,
        decimal amount,
        string currency,
        SubscriptionScope scope,
        Guid? scopeId,
        string provider,
        Guid paymentMethodId,
        string providerSubscriptionId,
        DateTime? nextChargeAt,
        string? externalOrderId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a payment subscription by its local Id or provider subscription Id.
    /// </summary>
    /// <param name="idOrProviderId">The local subscription Id or provider subscription Id (as Guid).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The <see cref="PaymentSubscription"/> if found; otherwise, null.</returns>
    Task<PaymentSubscription?> FindSubscriptionByIdOrProviderIdAsync(
        Guid idOrProviderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a payment subscription by its unique identifier for update operations.
    /// </summary>
    /// <remarks>This method is intended for scenarios where the subscription will be updated. The returned
    /// subscription may be locked for concurrency control depending on the underlying data store
    /// implementation.</remarks>
    /// <param name="subscriptionId">The unique identifier of the payment subscription to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="PaymentSubscription"/> instance representing the subscription if found; otherwise, <see
    /// langword="null"/>.</returns>
    Task<PaymentSubscription?> GetSubscriptionByIdForUpdateAsync(
    Guid subscriptionId,
    CancellationToken cancellationToken = default);
}
