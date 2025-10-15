namespace PetCare.Domain.Events;

/// <summary>
/// Represents an event that occurs when an adoption application has been approved.
/// </summary>
/// <param name="ApplicationId">The unique identifier of the adoption application that was approved.</param>
/// <param name="UserId">The unique identifier of the user who submitted the adoption application.</param>
/// <param name="AnimalId">The unique identifier of the animal associated with the adoption application.</param>
/// <param name="ApprovedBy">The unique identifier of the user or staff member who approved the adoption application.</param>
public sealed record AdoptionApplicationApprovedEvent(Guid ApplicationId, Guid UserId, Guid AnimalId, Guid ApprovedBy)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a new adoption application is created.
/// </summary>
/// <param name="ApplicationId">The unique identifier of the adoption application associated with this event.</param>
/// <param name="UserId">The unique identifier of the user who submitted the adoption application.</param>
/// <param name="AnimalId">The unique identifier of the animal for which the adoption application was submitted.</param>
public sealed record AdoptionApplicationCreatedEvent(Guid ApplicationId, Guid UserId, Guid AnimalId)
    : DomainEvent;

/// <summary>
/// Represents a domain event that occurs when the notes for an adoption application are updated.
/// </summary>
/// <param name="ApplicationId">The unique identifier of the adoption application whose notes have been updated.</param>
/// <param name="UserId">The unique identifier of the user who performed the update.</param>
/// <param name="Notes">The updated notes associated with the adoption application. Cannot be null.</param>
public sealed record AdoptionApplicationNotesUpdatedEvent(Guid ApplicationId, Guid UserId, string Notes)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when an adoption application is rejected.
/// </summary>
/// <param name="ApplicationId">The unique identifier of the adoption application that was rejected.</param>
/// <param name="UserId">The unique identifier of the user who submitted the adoption application.</param>
/// <param name="AnimalId">The unique identifier of the animal for which the adoption application was submitted.</param>
/// <param name="RejectionReason">The reason provided for rejecting the adoption application.</param>
public sealed record AdoptionApplicationRejectedEvent(Guid ApplicationId, Guid UserId, Guid AnimalId, string RejectionReason)
    : DomainEvent;
