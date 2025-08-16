namespace PetCare.Domain.Events;
using MediatR;

/// <summary>
/// Represents a base class for all domain events.
/// </summary>
public abstract record DomainEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEvent"/> class.
    /// </summary>
    protected DomainEvent()
    {
        this.Id = Guid.NewGuid();
        this.OccurredAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of the domain event.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; }
}
