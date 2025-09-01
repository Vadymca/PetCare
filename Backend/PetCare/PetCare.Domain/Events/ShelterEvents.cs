namespace PetCare.Domain.Events;

public sealed record ShelterCreatedEvent(Guid ShelterId)
    : DomainEvent;

public sealed record ShelterUpdatedEvent(Guid ShelterId)
    : DomainEvent;

public sealed record AnimalAddedToShelterEvent(Guid ShelterId, Guid AnimalId, int NewOccupancy)
    : DomainEvent;

public sealed record AnimalRemovedFromShelterEvent(Guid ShelterId, Guid AnimalId, int NewOccupancy)
    : DomainEvent;

public sealed record ShelterPhotoAddedEvent(Guid ShelterId, string PhotoUrl)
    : DomainEvent;

public sealed record ShelterPhotoRemovedEvent(Guid ShelterId, string PhotoUrl)
    : DomainEvent;

public sealed record ShelterSocialMediaAddedOrUpdatedEvent(Guid ShelterId, string Platform, string Url)
    : DomainEvent;

public sealed record ShelterSocialMediaRemovedEvent(Guid ShelterId, string Platform)
    : DomainEvent;

public sealed record DonationAddedToShelterEvent(Guid ShelterId, Guid DonationId)
    : DomainEvent;

public sealed record DonationRemovedFromShelterEvent(Guid ShelterId, Guid DonationId)
    : DomainEvent;

public record VolunteerTaskAddedToShelterEvent(Guid ShelterId, Guid TaskId)
    : DomainEvent;

public record VolunteerTaskRemovedFromShelterEvent(Guid ShelterId, Guid TaskId)
    : DomainEvent;

public record IoTDeviceAddedEvent(Guid ShelterId, Guid DeviceId)
     : DomainEvent;

public record IoTDeviceRemovedEvent(Guid ShelterId, Guid DeviceId)
     : DomainEvent;

public sealed record ShelterEventAddedEvent(Guid ShelterId, Guid EventId)
    : DomainEvent;

public sealed record ShelterEventRemovedEvent(Guid ShelterId, Guid EventId)
    : DomainEvent;