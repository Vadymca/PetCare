// <copyright file="INotificationDispatcher.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;

/// <summary>
/// Provides functionality to dispatch notifications to users.
/// </summary>
public interface INotificationDispatcher
{
    /// <summary>
    /// Sends a notification message to the specified user.
    /// </summary>
    /// <param name="userId">The identifier of the user to notify.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The content of the notification message.</param>
    /// <param name="notifiableEntity">The name of the related entity, if any (e.g. "Animal").</param>
    /// <param name="notifiableEntityId">The identifier of the related entity, if applicable.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    Task SendAsync(
        Guid userId,
        string title,
        string message,
        string? notifiableEntity = null,
        Guid? notifiableEntityId = null);
}
