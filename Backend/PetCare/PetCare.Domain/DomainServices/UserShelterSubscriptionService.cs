namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Implements domain service for managing user subscriptions to shelters.
/// </summary>
public class UserShelterSubscriptionService : IUserShelterSubscriptionService
{
    /// <inheritdoc />
    public Task AddShelterSubscriptionAsync(User user, ShelterSubscription subscription, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (subscription is null)
        {
            throw new ArgumentNullException(nameof(subscription));
        }

        user.AddShelterSubscription(subscription, requestingUserId);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> UpdateShelterSubscriptionDateAsync(User user, Guid shelterId, DateTime newSubscribedAt, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var result = user.UpdateShelterSubscriptionDate(shelterId, newSubscribedAt, requestingUserId);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<bool> RemoveShelterSubscriptionAsync(User user, Guid shelterId, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var result = user.RemoveShelterSubscription(shelterId, requestingUserId);
        return Task.FromResult(result);
    }
}
