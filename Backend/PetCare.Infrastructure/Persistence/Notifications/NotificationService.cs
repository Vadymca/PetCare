namespace PetCare.Infrastructure.Persistence.Notifications;

using PetCare.Domain.Abstractions.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Provides functionality to send notifications to users and moderators.
/// </summary>
public class NotificationService : INotificationService
{
    /// <summary>
    /// Sends a notification to all moderators.
    /// </summary>
    /// <param name="subject">The subject of the notification.</param>
    /// <param name="message">The message content.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>A completed task.</returns>
    public async Task NotifyModeratorsAsync(string subject, string message, CancellationToken cancellationToken)
    {
        // Не встиг добавити логіку
        await Task.CompletedTask;
    }

    /// <summary>
    /// Sends a notification to a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user to notify.</param>
    /// <param name="subject">The subject of the notification.</param>
    /// <param name="message">The message content.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task NotifyUserAsync(Guid userId, string subject, string message, CancellationToken cancellationToken)
    {
        // Не встиг добавити логіку
        await Task.CompletedTask;
    }
}
