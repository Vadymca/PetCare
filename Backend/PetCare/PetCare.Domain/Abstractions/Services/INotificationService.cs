namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Domain service for managing user notifications.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Adds a new notification for the user.
    /// </summary>
    /// <param name="user">The user aggregate to which the notification will be added.</param>
    /// <param name="notification">The notification to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the update. Must match the current user's ID or have elevated rights.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddNotificationAsync(User user, Notification notification, Guid requestingUserId);

    /// <summary>
    /// Removes a notification from the user.
    /// Only the user themselves or admins/moderators can perform this action.
    /// </summary>
    /// <param name="user">The user aggregate from which the notification will be removed.</param>
    /// <param name="notificationId">The unique identifier of the notification to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <returns>True if the notification was found and removed; otherwise, false.</returns>
    Task<bool> RemoveNotificationAsync(User user, Guid notificationId, Guid requestingUserId);
}
