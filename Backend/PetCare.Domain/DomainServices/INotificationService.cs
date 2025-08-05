// <copyright file="INotificationService.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;

/// <summary>
/// Provides methods to send notifications to users and moderators.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends a notification to all moderators.
    /// </summary>
    /// <param name="subject">The subject of the notification message.</param>
    /// <param name="message">The content of the notification message.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task NotifyModeratorsAsync(string subject, string message, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a notification to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to notify.</param>
    /// <param name="subject">The subject of the notification message.</param>
    /// <param name="message">The content of the notification message.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task NotifyUserAsync(Guid userId, string subject, string message, CancellationToken cancellationToken);
}
