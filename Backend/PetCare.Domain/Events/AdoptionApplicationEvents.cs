namespace PetCare.Domain.Events;

public sealed record AdoptionApplicationApprovedEvent(Guid applicationId, Guid userId, Guid animalId, Guid approvedBy)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AdoptionApplicationCreatedEvent(Guid applicationId, Guid userId, Guid animalId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AdoptionApplicationNotesUpdatedEvent(Guid applicationId, Guid userId, string notes)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AdoptionApplicationRejectedEvent(Guid applicationId, Guid userId, Guid animalId, string rejectionReason)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
