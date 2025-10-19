namespace PetCare.Domain.Events;

using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents an event that occurs when a new animal is created in the system.
/// </summary>
/// <param name="AnimalId">The unique identifier assigned to the newly created animal.</param>
/// <param name="Slug">The URL-friendly slug associated with the animal. Used for routing or identification in web contexts.</param>
/// <param name="Name">The display name of the animal.</param>
public sealed record AnimalCreatedEvent(Guid AnimalId, Slug Slug, Name Name)
   : DomainEvent;

/// <summary>
/// Represents an event that occurs when an animal entity is updated.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal that was updated.</param>
public sealed record AnimalUpdatedEvent(Guid AnimalId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when the status of an animal changes.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal whose status has changed.</param>
/// <param name="NewStatus">The new status assigned to the animal.</param>
public sealed record AnimalStatusChangedEvent(Guid AnimalId, AnimalStatus NewStatus)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a photo is added to an animal's record.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal to which the photo was added.</param>
/// <param name="PhotoUrl">The URL of the photo that was added to the animal's record. Cannot be null or empty.</param>
public sealed record AnimalPhotoAddedEvent(Guid AnimalId, string PhotoUrl)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a photo is removed from an animal's record.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal from which the photo was removed.</param>
/// <param name="PhotoUrl">The URL of the photo that was removed.</param>
public sealed record AnimalPhotoRemovedEvent(Guid AnimalId, string PhotoUrl)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a video is added to an animal's record.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal to which the video was added.</param>
/// <param name="VideoUrl">The URL of the video that was added to the animal's record.</param>
public sealed record AnimalVideoAddedEvent(Guid AnimalId, string VideoUrl)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a video associated with an animal is removed.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal whose video was removed.</param>
/// <param name="VideoUrl">The URL of the video that was removed from the animal's record.</param>
public sealed record AnimalVideoRemovedEvent(Guid AnimalId, string VideoUrl)
    : DomainEvent;

/// <summary>
/// Represents a domain event indicating that a user has subscribed to receive updates about a specific animal.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal to which the user has subscribed.</param>
/// <param name="UserId">The unique identifier of the user who has subscribed to the animal event.</param>
public sealed record UserSubscribedToAnimalEvent(Guid AnimalId, Guid UserId)
    : DomainEvent;

/// <summary>
/// Represents an event indicating that a user has unsubscribed from notifications or updates related to a specific
/// animal.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal from which the user has unsubscribed.</param>
/// <param name="UserId">The unique identifier of the user who has unsubscribed from the animal.</param>
public sealed record UserUnsubscribedFromAnimalEvent(Guid AnimalId, Guid UserId)
    : DomainEvent;
