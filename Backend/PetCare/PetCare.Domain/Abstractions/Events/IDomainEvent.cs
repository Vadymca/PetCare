namespace PetCare.Domain.Abstractions.Events;

using System;
using MediatR;

/// <summary>
/// Represents a marker interface for all domain events.
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Gets the unique identifier of the domain event.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    DateTime OccurredAt { get; }
}
