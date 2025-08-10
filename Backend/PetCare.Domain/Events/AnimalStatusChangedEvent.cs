namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when an animal's status changes.
/// </summary>
public sealed record AnimalStatusChangedEvent(Guid animalId, string oldStatus, string newStatus)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
