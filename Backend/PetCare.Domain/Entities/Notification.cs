// <copyright file="Notification.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a notification in the system.
/// </summary>
public sealed class Notification : BaseEntity
{
    private Notification()
    {
    }

    private Notification(
       Guid userId,
       Guid notificationTypeId,
       Title title,
       string message,
       string? notifiableEntity,
       Guid? notifiableEntityId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        if (notificationTypeId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор типу сповіщення не може бути порожнім.", nameof(notificationTypeId));
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Повідомлення не може бути порожнім.", nameof(message));
        }

        this.UserId = userId;
        this.NotificationTypeId = notificationTypeId;
        this.Title = title;
        this.Message = message;
        this.NotifiableEntity = notifiableEntity;
        this.NotifiableEntityId = notifiableEntityId;
        this.IsRead = false;
        this.CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the title of the notification.
    /// </summary>
    public Title Title { get; private set; } = default!;

    /// <summary>
    /// Gets the message content of the notification.
    /// </summary>
    public string Message { get; private set; } = default!;

    /// <summary>
    /// Gets a value indicating whether the notification has been read.
    /// </summary>
    public bool IsRead { get; private set; }

    /// <summary>
    /// Gets the entity type associated with the notification, if any. Can be null.
    /// </summary>
    public string? NotifiableEntity { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the associated entity, if any. Can be null.
    /// </summary>
    public Guid? NotifiableEntityId { get; private set; }

    /// <summary>
    /// Gets the date and time when the notification was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user receiving the notification.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the user who receives the notification.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the notification type.
    /// </summary>
    public Guid NotificationTypeId { get; private set; }

    /// <summary>
    /// Gets the notification type.
    /// </summary>
    public NotificationType? NotificationType { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Notification"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user receiving the notification.</param>
    /// <param name="notificationTypeId">The unique identifier of the notification type.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The message content of the notification.</param>
    /// <param name="notifiableEntity">The entity type associated with the notification, if any. Can be null.</param>
    /// <param name="notifiableEntityId">The unique identifier of the associated entity, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="Notification"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> or <paramref name="notificationTypeId"/> is an empty GUID, or <paramref name="message"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to the <see cref="Title.Create"/> method.</exception>
    public static Notification Create(
        Guid userId,
        Guid notificationTypeId,
        string title,
        string message,
        string? notifiableEntity = null,
        Guid? notifiableEntityId = null)
    {
        return new Notification(
            userId,
            notificationTypeId,
            Title.Create(title),
            message,
            notifiableEntity,
            notifiableEntityId);
    }

    /// <summary>
    /// Marks the notification as read.
    /// </summary>
    public void MarkAsRead()
    {
        this.IsRead = true;
    }
}
