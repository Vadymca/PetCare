namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when an adoption application is approved.
/// </summary>
public sealed record AdoptionApplicationApprovedEvent(Guid applicationId, Guid userId, Guid animalId, Guid approvedBy)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

/// <summary>
/// Event raised when a new adoption application is created.
/// </summary>
public sealed record AdoptionApplicationCreatedEvent(Guid applicationId, Guid userId, Guid animalId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

/// <summary>
/// Event raised when administrative notes are added or updated for an adoption application.
/// </summary>
public sealed record AdoptionApplicationNotesUpdatedEvent(Guid applicationId, Guid userId, string notes)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

/// <summary>
/// Event raised when an adoption application is rejected.
/// </summary>
public sealed record AdoptionApplicationRejectedEvent(Guid applicationId, Guid userId, Guid animalId, string rejectionReason)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
