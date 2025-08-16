namespace PetCare.Domain.Events;

using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

public sealed record AnimalCreatedEvent(Guid animalId, Slug slug, Name name)
   : DomainEvent;

public sealed record AnimalUpdatedEvent(Guid animalId)
    : DomainEvent;

public sealed record AnimalStatusChangedEvent(Guid animalId, AnimalStatus newStatus)
    : DomainEvent;

public sealed record AnimalPhotoAddedEvent(Guid animalId, string photoUrl)
    : DomainEvent;

public sealed record AnimalPhotoRemovedEvent(Guid animalId, string photoUrl)
    : DomainEvent;

public sealed record AnimalVideoAddedEvent(Guid animalId, string videoUrl)
    : DomainEvent;

public sealed record AnimalVideoRemovedEvent(Guid animalId, string videoUrl)
    : DomainEvent;
