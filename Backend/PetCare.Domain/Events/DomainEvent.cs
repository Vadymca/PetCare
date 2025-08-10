namespace PetCare.Domain.Events;

/// <summary>
/// Represents a base domain event.
/// </summary>
public abstract record DomainEvent(Guid id, DateTime occurredAt);
