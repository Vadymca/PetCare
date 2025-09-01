namespace PetCare.Domain.Events;

using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

public sealed record AnimalCreatedEvent(Guid AnimalId, Slug Slug, Name Name)
   : DomainEvent;

public sealed record AnimalUpdatedEvent(Guid AnimalId)
    : DomainEvent;

public sealed record AnimalStatusChangedEvent(Guid AnimalId, AnimalStatus NewStatus)
    : DomainEvent;

public sealed record AnimalPhotoAddedEvent(Guid AnimalId, string PhotoUrl)
    : DomainEvent;

public sealed record AnimalPhotoRemovedEvent(Guid AnimalId, string PhotoUrl)
    : DomainEvent;

public sealed record AnimalVideoAddedEvent(Guid AnimalId, string VideoUrl)
    : DomainEvent;

public sealed record AnimalVideoRemovedEvent(Guid AnimalId, string VideoUrl)
    : DomainEvent;
