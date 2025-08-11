namespace PetCare.Domain.Events;

using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

public sealed record AnimalCreatedEvent(Guid animalId, Slug slug, Name name)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalUpdatedEvent(Guid animalId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalStatusChangedEvent(Guid animalId, AnimalStatus newStatus)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalPhotoAddedEvent(Guid animalId, string photoUrl)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalPhotoRemovedEvent(Guid animalId, string photoUrl)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalVideoAddedEvent(Guid animalId, string videoUrl)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalVideoRemovedEvent(Guid animalId, string videoUrl)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
