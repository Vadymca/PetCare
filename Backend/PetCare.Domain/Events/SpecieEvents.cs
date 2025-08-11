namespace PetCare.Domain.Events;

public sealed record SpecieCreatedEvent(Guid specieId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record SpecieRenamedEvent(Guid specieId, string newName)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record BreedAddedEvent(Guid specieId, Guid breedId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record BreedRemovedEvent(Guid specieId, Guid breedId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
