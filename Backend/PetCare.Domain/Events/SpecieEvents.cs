namespace PetCare.Domain.Events;

public sealed record SpecieCreatedEvent(Guid specieId)
    : DomainEvent;

public sealed record SpecieRenamedEvent(Guid specieId, string newName)
    : DomainEvent;

public sealed record BreedAddedEvent(Guid specieId, Guid breedId)
    : DomainEvent;

public sealed record BreedRemovedEvent(Guid specieId, Guid breedId)
    : DomainEvent;
