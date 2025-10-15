namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Represents a domain service for managing user subscriptions to shelters.
/// </summary>
public interface IUserShelterSubscriptionService
{
    /// <summary>
    /// Adds a shelter subscription for the user.
    /// </summary>
    /// <param name="user">The user to subscribe.</param>
    /// <param name="subscription">The shelter subscription to add.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddShelterSubscriptionAsync(User user, ShelterSubscription subscription, Guid requestingUserId);

    /// <summary>
    /// Updates the subscription date for a specific shelter.
    /// </summary>
    /// <param name="user">The user whose subscription will be updated.</param>
    /// <param name="shelterId">The ID of the shelter.</param>
    /// <param name="newSubscribedAt">The new subscription date.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation.</param>
    /// <returns>True if updated successfully, otherwise false.</returns>
    Task<bool> UpdateShelterSubscriptionDateAsync(User user, Guid shelterId, DateTime newSubscribedAt, Guid requestingUserId);

    /// <summary>
    /// Removes a shelter subscription for the user.
    /// </summary>
    /// <param name="user">The user whose subscription will be removed.</param>
    /// <param name="shelterId">The ID of the shelter.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation.</param>
    /// <returns>True if removed successfully, otherwise false.</returns>
    Task<bool> RemoveShelterSubscriptionAsync(User user, Guid shelterId, Guid requestingUserId);
}
