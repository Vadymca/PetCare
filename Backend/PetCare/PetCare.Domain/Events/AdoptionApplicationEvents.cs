namespace PetCare.Domain.Events;

public sealed record AdoptionApplicationApprovedEvent(Guid ApplicationId, Guid UserId, Guid AnimalId, Guid ApprovedBy)
    : DomainEvent;

public sealed record AdoptionApplicationCreatedEvent(Guid ApplicationId, Guid UserId, Guid AnimalId)
    : DomainEvent;

public sealed record AdoptionApplicationNotesUpdatedEvent(Guid ApplicationId, Guid UserId, string Notes)
    : DomainEvent;

public sealed record AdoptionApplicationRejectedEvent(Guid ApplicationId, Guid UserId, Guid AnimalId, string RejectionReason)
    : DomainEvent;
