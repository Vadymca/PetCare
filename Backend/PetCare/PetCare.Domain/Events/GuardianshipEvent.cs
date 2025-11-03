namespace PetCare.Domain.Events;

/// <summary>
/// Represents an event that occurs when a new guardianship relationship is created between a user and an animal.
/// </summary>
/// <param name="GuardianshipId">The unique identifier for the guardianship relationship associated with this event.</param>
/// <param name="UserId">The unique identifier of the user who is assigned as the guardian.</param>
/// <param name="AnimalId">The unique identifier of the animal for which the guardianship is created.</param>
public sealed record GuardianshipCreatedEvent(Guid GuardianshipId, Guid UserId, Guid AnimalId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a guardianship is activated for a specific user and animal.
/// </summary>
/// <param name="GuardianshipId">The unique identifier of the guardianship that has been activated.</param>
/// <param name="UserId">The unique identifier of the user associated with the activated guardianship.</param>
/// <param name="AnimalId">The unique identifier of the animal associated with the activated guardianship.</param>
public sealed record GuardianshipActivatedEvent(Guid GuardianshipId, Guid UserId, Guid AnimalId)
    : DomainEvent;

/// <summary>
/// Represents an event indicating that a guardianship requires payment, including the relevant guardianship, user,
/// animal, and the grace period deadline.
/// </summary>
/// <param name="GuardianshipId">The unique identifier of the guardianship for which payment is required.</param>
/// <param name="UserId">The unique identifier of the user associated with the guardianship.</param>
/// <param name="AnimalId">The unique identifier of the animal involved in the guardianship.</param>
/// <param name="GraceUntilUtc">The date and time, in UTC, until which payment can be made before further action is taken.</param>
public sealed record GuardianshipRequiresPaymentEvent(Guid GuardianshipId, Guid UserId, Guid AnimalId, DateTime GraceUntilUtc)
    : DomainEvent;

/// <summary>
/// Represents an event that is raised when a guardianship process has been completed for a specific animal and user.
/// </summary>
/// <param name="GuardianshipId">The unique identifier of the completed guardianship.</param>
/// <param name="UserId">The unique identifier of the user associated with the guardianship.</param>
/// <param name="AnimalId">The unique identifier of the animal for which the guardianship was completed.</param>
public sealed record GuardianshipCompletedEvent(Guid GuardianshipId, Guid UserId, Guid AnimalId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a donation is added to a guardianship.
/// </summary>
/// <param name="GuardianshipId">The unique identifier of the guardianship to which the donation was added.</param>
/// <param name="DonationId">The unique identifier of the donation that was added.</param>
public sealed record GuardianshipDonationAddedEvent(Guid GuardianshipId, Guid DonationId)
    : DomainEvent;
