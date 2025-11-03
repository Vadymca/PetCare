namespace PetCare.Domain.Abstractions.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Defines a contract for accessing and managing guardianship entities, including retrieval, listing, and association
/// of related data such as users, animals, and donations.
/// </summary>
/// <remarks>This repository interface provides methods for querying guardianships with various filters and detail
/// levels, as well as for linking donations to guardianships. Methods support asynchronous operations and allow for
/// cancellation via a CancellationToken. Implementations are expected to handle entity tracking and related data
/// loading as described in each method's contract.</remarks>
public interface IGuardianshipRepository : IRepository<Guardianship>
{
   /// <summary>
   /// Asynchronously retrieves a guardianship entity by its unique identifier, including all related details.
   /// </summary>
   /// <param name="id">The unique identifier of the guardianship to retrieve.</param>
   /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
   /// <returns>A task that represents the asynchronous operation. The task result contains the guardianship entity with related
   /// details if found; otherwise, null.</returns>
    Task<Guardianship?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a guardianship entity by its unique identifier for update operations.
    /// </summary>
    /// <remarks>This method is intended for scenarios where the retrieved entity will be updated. The
    /// returned entity may be tracked for changes by the underlying data context.</remarks>
    /// <param name="id">The unique identifier of the guardianship to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the guardianship entity if found;
    /// otherwise, null.</returns>
    Task<Guardianship?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves the active guardianship record for the specified user and animal, if one exists.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom to retrieve the guardianship.</param>
    /// <param name="animalId">The unique identifier of the animal associated with the guardianship.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the active guardianship for the
    /// specified user and animal, or <see langword="null"/> if no active guardianship exists.</returns>
    Task<Guardianship?> GetActiveByUserAndAnimalAsync(Guid userId, Guid animalId, CancellationToken ct = default);

    /// <summary>
    /// Determines whether there is an active record associated with the specified user and animal.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to check for an active record.</param>
    /// <param name="animalId">The unique identifier of the animal to check for an active record.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if an active
    /// record exists for the specified user and animal; otherwise, <see langword="false"/>.</returns>
    Task<bool> ExistsActiveByUserAndAnimalAsync(Guid userId, Guid animalId, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of guardianships associated with the specified user, optionally
    /// filtered by guardianship status.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose guardianships are to be retrieved.</param>
    /// <param name="status">An optional status value to filter the guardianships. If null, all guardianships for the user are returned
    /// regardless of status.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianships
    /// for the specified user. The list is empty if no matching guardianships are found.</returns>
    Task<IReadOnlyList<Guardianship>> ListByUserAsync(Guid userId, GuardianshipStatus? status = null, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of guardianship records associated with the specified animal,
    /// optionally filtered by guardianship status.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal for which to retrieve guardianship records.</param>
    /// <param name="status">An optional status value to filter the guardianship records. If null, all statuses are included.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianship
    /// records matching the specified criteria. The list is empty if no records are found.</returns>
    Task<IReadOnlyList<Guardianship>> ListByAnimalAsync(Guid animalId, GuardianshipStatus? status = null, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of guardianships that have expired and require payment as of the
    /// specified UTC date and time.
    /// </summary>
    /// <param name="utcNow">The current date and time, in Coordinated Universal Time (UTC), used to determine which guardianships are
    /// considered expired.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianships
    /// that are expired and require payment at the specified time. The list will be empty if no such guardianships are
    /// found.</returns>
    Task<IReadOnlyList<Guardianship>> ListExpiredRequiresPaymentAsync(DateTime utcNow, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of guardianships that require payment within the specified UTC date
    /// and time range.
    /// </summary>
    /// <param name="fromUtc">The start of the date and time range, in UTC, to search for guardianships requiring payment. Must be less than
    /// or equal to <paramref name="toUtc"/>.</param>
    /// <param name="toUtc">The end of the date and time range, in UTC, to search for guardianships requiring payment. Must be greater than
    /// or equal to <paramref name="fromUtc"/>.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of <see
    /// cref="Guardianship"/> objects that require payment within the specified range. The list is empty if no matching
    /// guardianships are found.</returns>
    Task<IReadOnlyList<Guardianship>> ListRequiresPaymentWithinAsync(DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously links a donation to a specified guardianship record.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship to which the donation will be linked.</param>
    /// <param name="donationId">The unique identifier of the donation to be linked to the guardianship.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous link operation.</returns>
    Task LinkDonationAsync(Guid guardianshipId, Guid donationId, CancellationToken ct = default);

    /// <summary>
    /// Cancels the active subscription associated with the specified guardianship asynchronously.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship whose subscription is to be canceled.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous cancellation operation.</returns>
    Task CancelSubscriptionAsync(Guid guardianshipId, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously adds a new donation record to the data store.
    /// </summary>
    /// <param name="donation">The donation to add. Cannot be null. All required fields of the donation must be populated.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddDonationAsync(Donation donation, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the unique identifier of a payment method associated with the specified provider, ensuring that a
    /// valid payment method exists for the provider.
    /// </summary>
    /// <param name="provider">The name of the payment provider for which to retrieve the payment method identifier. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the payment
    /// method for the specified provider.</returns>
    Task<Guid> RequirePaymentMethodIdByProviderAsync(string provider, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of donations made by the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose donation records are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of <see
    /// cref="Donation"/> objects associated with the specified user. If the user has not made any donations, the list
    /// will be empty.</returns>
    Task<IReadOnlyList<Donation>> GetUserDonationsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new payment subscription to the system.
    /// </summary>
    /// <param name="subscription">The payment subscription to add. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddSubscriptionAsync(PaymentSubscription subscription, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels an active subscription identified by the specified provider subscription ID.
    /// </summary>
    /// <param name="providerSubscriptionId">The unique identifier of the subscription to cancel, as provided by the external provider. Cannot be null or
    /// empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous cancellation operation.</returns>
    Task CancelSubscriptionByProviderIdAsync(string providerSubscriptionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels all active subscriptions within the specified scope asynchronously.
    /// </summary>
    /// <remarks>Use this method to bulk cancel subscriptions based on a defined scope, such as user, group,
    /// or organization. The operation is performed asynchronously and may affect multiple subscriptions depending on
    /// the scope and scope identifier provided.</remarks>
    /// <param name="scope">The scope in which subscriptions will be cancelled. Determines the boundary for the cancellation operation.</param>
    /// <param name="scopeId">The unique identifier of the scope. If null, the operation applies to all subscriptions within the specified
    /// scope type.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be cancelled if this token is triggered.</param>
    /// <returns>A task that represents the asynchronous cancellation operation.</returns>
    Task CancelSubscriptionsByScopeAsync(SubscriptionScope scope, Guid? scopeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all payment subscriptions associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose payment subscriptions are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of payment subscriptions for the specified user. The list will be empty if the user has no
    /// subscriptions.</returns>
    Task<IReadOnlyList<PaymentSubscription>> GetUserSubscriptionsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the list of payment subscriptions for the specified user, along with the expected date of the next
    /// charge for each subscription.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose expected charges are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of tuples, each containing a payment subscription and the expected date of the next charge. The
    /// date is <see langword="null"/> if no upcoming charge is scheduled for the subscription.</returns>
    Task<IReadOnlyList<(PaymentSubscription Subscription, DateTime? NextChargeAt)>> GetUserExpectedChargesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a payment subscription that matches the specified provider subscription identifier.
    /// </summary>
    /// <param name="providerSubscriptionId">The unique identifier assigned to the subscription by the external payment provider. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see
    /// cref="PaymentSubscription"/> if found; otherwise, <see langword="null"/>.</returns>
    Task<PaymentSubscription?> FindByProviderSubscriptionIdAsync(
        string providerSubscriptionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of all payment subscriptions.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of all payment
    /// subscriptions.</returns>
    Task<IReadOnlyList<PaymentSubscription>> ListAllSubscriptionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of all donations.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of all donations.</returns>
    Task<IReadOnlyList<Donation>> ListAllDonationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of donations associated with the specified project.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project for which to list donations.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of donations for
    /// the specified project. If no donations exist, the list will be empty.</returns>
    Task<IReadOnlyList<Donation>> ListDonationsByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of all available payment methods.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of all payment
    /// methods. If no payment methods are available, the list will be empty.</returns>
    Task<IReadOnlyList<PaymentMethod>> ListAllPaymentMethodsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the payment method associated with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the payment method to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the payment method if found;
    /// otherwise, <see langword="null"/>.</returns>
    Task<PaymentMethod?> GetPaymentMethodByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a payment method by its name.
    /// </summary>
    /// <param name="name">The name of the payment method to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the payment method matching the
    /// specified name, or null if no such payment method exists.</returns>
    Task<PaymentMethod?> GetPaymentMethodByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new payment method to the user's account.
    /// </summary>
    /// <param name="method">The payment method to add. Must not be null. The details of the payment method are used to associate it with the
    /// user's account.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddPaymentMethodAsync(PaymentMethod method, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the specified payment method with new information.
    /// </summary>
    /// <param name="method">The payment method to update. Cannot be null. The provided object must contain the updated details to be
    /// applied.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdatePaymentMethodAsync(PaymentMethod method, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified payment method asynchronously from the system.
    /// </summary>
    /// <param name="method">The payment method to be deleted. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeletePaymentMethodAsync(PaymentMethod method, CancellationToken cancellationToken = default);
}
