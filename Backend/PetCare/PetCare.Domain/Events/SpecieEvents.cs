namespace PetCare.Domain.Events;

public sealed record SpecieCreatedEvent(Guid SpecieId)
    : DomainEvent;

public sealed record SpecieRenamedEvent(Guid SpecieId, string NewName)
    : DomainEvent;

public sealed record BreedAddedEvent(Guid SpecieId, Guid BreedId)
    : DomainEvent;

public sealed record BreedRemovedEvent(Guid SpecieId, Guid BreedId)
    : DomainEvent;
