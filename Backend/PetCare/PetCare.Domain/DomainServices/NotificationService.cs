namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Implementation of the domain service for managing notifications.
/// </summary>
public sealed class NotificationService : INotificationService
{
    /// <inheritdoc/>
    public async Task AddNotificationAsync(User user, Notification notification, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        user.AddNotification(notification, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveNotificationAsync(User user, Guid notificationId, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var removed = user.RemoveNotification(notificationId, requestingUserId);
        return await Task.FromResult(removed);
    }
}
