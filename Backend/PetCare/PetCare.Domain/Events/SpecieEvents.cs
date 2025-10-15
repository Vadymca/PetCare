namespace PetCare.Domain.Events;

/// <summary>
/// Represents an event that occurs when a new species is created.
/// </summary>
/// <param name="SpecieId">The unique identifier of the species that was created.</param>
public sealed record SpecieCreatedEvent(Guid SpecieId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a species is renamed.
/// </summary>
/// <param name="SpecieId">The unique identifier of the species that was renamed.</param>
/// <param name="NewName">The new name assigned to the species. Cannot be null or empty.</param>
public sealed record SpecieRenamedEvent(Guid SpecieId, string NewName)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a new breed is added to a species.
/// </summary>
/// <param name="SpecieId">The unique identifier of the species to which the breed is added.</param>
/// <param name="BreedId">The unique identifier of the breed that has been added.</param>
public sealed record BreedAddedEvent(Guid SpecieId, Guid BreedId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a breed is removed from a species.
/// </summary>
/// <param name="SpecieId">The unique identifier of the species from which the breed was removed.</param>
/// <param name="BreedId">The unique identifier of the breed that was removed.</param>
public sealed record BreedRemovedEvent(Guid SpecieId, Guid BreedId)
    : DomainEvent;
