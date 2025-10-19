namespace PetCare.Domain.Events;

/// <summary>
/// Represents an event that occurs when a new shelter is created.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter that was created.</param>
public sealed record ShelterCreatedEvent(Guid ShelterId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a shelter is updated.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter that was updated.</param>
public sealed record ShelterUpdatedEvent(Guid ShelterId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a new animal is added to a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter to which the animal was added.</param>
/// <param name="AnimalId">The unique identifier of the animal that was added to the shelter.</param>
/// <param name="NewOccupancy">The total number of animals in the shelter after the addition.</param>
public sealed record AnimalAddedToShelterEvent(Guid ShelterId, Guid AnimalId, int NewOccupancy)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when an animal is removed from a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the animal was removed.</param>
/// <param name="AnimalId">The unique identifier of the animal that was removed.</param>
/// <param name="NewOccupancy">The new total number of animals remaining in the shelter after the removal. Must be zero or greater.</param>
public sealed record AnimalRemovedFromShelterEvent(Guid ShelterId, Guid AnimalId, int NewOccupancy)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a new photo is added to a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter to which the photo was added.</param>
/// <param name="PhotoUrl">The URL of the photo that was added to the shelter.</param>
public sealed record ShelterPhotoAddedEvent(Guid ShelterId, string PhotoUrl)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a photo is removed from a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the photo was removed.</param>
/// <param name="PhotoUrl">The URL of the photo that was removed from the shelter.</param>
public sealed record ShelterPhotoRemovedEvent(Guid ShelterId, string PhotoUrl)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a shelter's social media information is added or updated.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter whose social media information has changed.</param>
/// <param name="Platform">The name of the social media platform (for example, "Facebook" or "Twitter").</param>
/// <param name="Url">The URL of the shelter's profile or page on the specified social media platform.</param>
public sealed record ShelterSocialMediaAddedOrUpdatedEvent(Guid ShelterId, string Platform, string Url)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a social media account is removed from a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the social media account was removed.</param>
/// <param name="Platform">The name of the social media platform that was removed from the shelter.</param>
public sealed record ShelterSocialMediaRemovedEvent(Guid ShelterId, string Platform)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a donation is added to a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter to which the donation was added.</param>
/// <param name="DonationId">The unique identifier of the donation that was added.</param>
public sealed record DonationAddedToShelterEvent(Guid ShelterId, Guid DonationId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a donation is removed from a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the donation was removed.</param>
/// <param name="DonationId">The unique identifier of the donation that was removed.</param>
public sealed record DonationRemovedFromShelterEvent(Guid ShelterId, Guid DonationId)
    : DomainEvent;

/// <summary>
/// Represents the event that occurs when a volunteer task is added to a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter to which the volunteer task was added.</param>
/// <param name="TaskId">The unique identifier of the volunteer task that was added.</param>
public record VolunteerTaskAddedToShelterEvent(Guid ShelterId, Guid TaskId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a volunteer task is removed from a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the volunteer task was removed.</param>
/// <param name="TaskId">The unique identifier of the volunteer task that was removed.</param>
public record VolunteerTaskRemovedFromShelterEvent(Guid ShelterId, Guid TaskId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a new IoT device is added to a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter to which the device was added.</param>
/// <param name="DeviceId">The unique identifier of the IoT device that was added.</param>
public record IoTDeviceAddedEvent(Guid ShelterId, Guid DeviceId)
     : DomainEvent;

/// <summary>
/// Represents an event that occurs when an IoT device is removed from a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the device was removed.</param>
/// <param name="DeviceId">The unique identifier of the IoT device that was removed.</param>
public record IoTDeviceRemovedEvent(Guid ShelterId, Guid DeviceId)
     : DomainEvent;

/// <summary>
/// Represents the occurrence of a new event being added to a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter to which the event was added.</param>
/// <param name="EventId">The unique identifier of the event that was added.</param>
public sealed record ShelterEventAddedEvent(Guid ShelterId, Guid EventId)
    : DomainEvent;

/// <summary>
/// Represents a domain event that occurs when an event is removed from a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the event was removed.</param>
/// <param name="EventId">The unique identifier of the event that was removed.</param>
public sealed record ShelterEventRemovedEvent(Guid ShelterId, Guid EventId)
    : DomainEvent;

/// <summary>
/// Represents a domain event indicating that a user has subscribed to notifications or updates from a specific shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter to which the user has subscribed.</param>
/// <param name="UserId">The unique identifier of the user who has subscribed to the shelter event.</param>
public sealed record UserSubscribedToShelterEvent(Guid ShelterId, Guid UserId)
    : DomainEvent;

/// <summary>
/// Represents an event indicating that a user has unsubscribed from notifications or updates for a specific shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the user has unsubscribed.</param>
/// <param name="UserId">The unique identifier of the user who has unsubscribed from the shelter.</param>
public sealed record UserUnsubscribedFromShelterEvent(Guid ShelterId, Guid UserId)
    : DomainEvent;