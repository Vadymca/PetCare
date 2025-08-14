namespace PetCare.Domain.Events;

public sealed record ShelterCreatedEvent(Guid shelterId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterUpdatedEvent(Guid shelterId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalAddedToShelterEvent(Guid shelterId, Guid animalId, int newOccupancy)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalRemovedFromShelterEvent(Guid shelterId, Guid animalId, int newOccupancy)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterPhotoAddedEvent(Guid shelterId, string photoUrl)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterPhotoRemovedEvent(Guid shelterId, string photoUrl)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterSocialMediaAddedOrUpdatedEvent(Guid shelterId, string platform, string url)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterSocialMediaRemovedEvent(Guid shelterId, string platform)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record DonationAddedToShelterEvent(Guid shelterId, Guid donationId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record DonationRemovedFromShelterEvent(Guid shelterId, Guid donationId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record VolunteerTaskAddedToShelterEvent(Guid shelterId, Guid taskId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record VolunteerTaskRemovedFromShelterEvent(Guid shelterId, Guid taskId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record IoTDeviceAddedEvent(Guid shelterId, Guid deviceId)
     : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record IoTDeviceRemovedEvent(Guid shelterId, Guid deviceId)
     : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterEventAddedEvent(Guid shelterId, Guid eventId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterEventRemovedEvent(Guid shelterId, Guid eventId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);