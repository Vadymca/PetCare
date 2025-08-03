// <copyright file="EventParticipant.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;

/// <summary>
/// Represents a value object that associates a user with an event as a participant.
/// </summary>
public sealed class EventParticipant : ValueObject
{
    private EventParticipant()
    {
    }

    private EventParticipant(Guid eventId, Guid userId, DateTime registeredAt)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор події не може бути порожнім.", nameof(eventId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        this.EventId = eventId;
        this.UserId = userId;
        this.RegisteredAt = registeredAt;
    }

    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user participating in the event.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the date and time when the user registered for the event.
    /// </summary>
    public DateTime RegisteredAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="EventParticipant"/> instance with the specified parameters.
    /// </summary>
    /// <param name="eventId">The unique identifier of the event.</param>
    /// <param name="userId">The unique identifier of the user participating in the event.</param>
    /// <returns>A new instance of <see cref="EventParticipant"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="eventId"/> or <paramref name="userId"/> is empty.</exception>
    public static EventParticipant Create(Guid eventId, Guid userId) =>
        new EventParticipant(eventId, userId, DateTime.UtcNow);

    /// <summary>
    /// Returns the components used to determine equality for the <see cref="EventParticipant"/> instance.
    /// </summary>
    /// <returns>An enumerable of objects representing the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.EventId;
        yield return this.UserId;
    }
}
