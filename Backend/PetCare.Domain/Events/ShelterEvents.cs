namespace PetCare.Domain.Events;

public sealed record ShelterCreatedEvent(Guid shelterId)
    : DomainEvent;

public sealed record ShelterUpdatedEvent(Guid shelterId)
    : DomainEvent;

public sealed record AnimalAddedToShelterEvent(Guid shelterId, Guid animalId, int newOccupancy)
    : DomainEvent;

public sealed record AnimalRemovedFromShelterEvent(Guid shelterId, Guid animalId, int newOccupancy)
    : DomainEvent;

public sealed record ShelterPhotoAddedEvent(Guid shelterId, string photoUrl)
    : DomainEvent;

public sealed record ShelterPhotoRemovedEvent(Guid shelterId, string photoUrl)
    : DomainEvent;

public sealed record ShelterSocialMediaAddedOrUpdatedEvent(Guid shelterId, string platform, string url)
    : DomainEvent;

public sealed record ShelterSocialMediaRemovedEvent(Guid shelterId, string platform)
    : DomainEvent;

public sealed record DonationAddedToShelterEvent(Guid shelterId, Guid donationId)
    : DomainEvent;

public sealed record DonationRemovedFromShelterEvent(Guid shelterId, Guid donationId)
    : DomainEvent;

public record VolunteerTaskAddedToShelterEvent(Guid shelterId, Guid taskId)
    : DomainEvent;

public record VolunteerTaskRemovedFromShelterEvent(Guid shelterId, Guid taskId)
    : DomainEvent;

public record IoTDeviceAddedEvent(Guid shelterId, Guid deviceId)
     : DomainEvent;

public record IoTDeviceRemovedEvent(Guid shelterId, Guid deviceId)
     : DomainEvent;

public sealed record ShelterEventAddedEvent(Guid shelterId, Guid eventId)
    : DomainEvent;

public sealed record ShelterEventRemovedEvent(Guid shelterId, Guid eventId)
    : DomainEvent;