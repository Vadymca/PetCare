using PetCare.Domain.Common;
using PetCare.Domain.Enums;

namespace PetCare.Domain.Events
{
    public sealed class AnimalCreatedEvent : DomainEventBase
    {
        public string Name { get; }
        public Guid BreedId { get; }
        public Guid ShelterId { get; }
        public AnimalStatus Status { get; }

        public AnimalCreatedEvent(Guid aggregateId, int aggregateVersion, string name, Guid breedId, Guid shelterId, AnimalStatus status)
            : base(aggregateId, aggregateVersion)
        {
            Name = name;
            BreedId = breedId;
            ShelterId = shelterId;
            Status = status;
        }
    }

    public sealed class AnimalStatusChangedEvent : DomainEventBase
    {
        public AnimalStatus PreviousStatus { get; }
        public AnimalStatus NewStatus { get; }
        public string? Reason { get; }

        public AnimalStatusChangedEvent(Guid aggregateId, int aggregateVersion, AnimalStatus previousStatus, AnimalStatus newStatus, string? reason = null)
            : base(aggregateId, aggregateVersion)
        {
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
            Reason = reason;
        }
    }

    public sealed class AnimalAdoptedEvent : DomainEventBase
    {
        public Guid AdopterId { get; }
        public DateTime AdoptionDate { get; }

        public AnimalAdoptedEvent(Guid aggregateId, int aggregateVersion, Guid adopterId, DateTime adoptionDate)
            : base(aggregateId, aggregateVersion)
        {
            AdopterId = adopterId;
            AdoptionDate = adoptionDate;
        }
    }

    public sealed class AnimalHealthStatusUpdatedEvent : DomainEventBase
    {
        public string? PreviousHealthStatus { get; }
        public string? NewHealthStatus { get; }

        public AnimalHealthStatusUpdatedEvent(Guid aggregateId, int aggregateVersion, string? previousHealthStatus, string? newHealthStatus)
            : base(aggregateId, aggregateVersion)
        {
            PreviousHealthStatus = previousHealthStatus;
            NewHealthStatus = newHealthStatus;
        }
    }

    public sealed class AnimalMicrochipAddedEvent : DomainEventBase
    {
        public string MicrochipId { get; }

        public AnimalMicrochipAddedEvent(Guid aggregateId, int aggregateVersion, string microchipId)
            : base(aggregateId, aggregateVersion)
        {
            MicrochipId = microchipId;
        }
    }

    public sealed class AnimalPhysicalCharacteristicsUpdatedEvent : DomainEventBase
    {
        public float? Weight { get; }
        public float? Height { get; }
        public string? Color { get; }

        public AnimalPhysicalCharacteristicsUpdatedEvent(Guid aggregateId, int aggregateVersion, float? weight, float? height, string? color)
            : base(aggregateId, aggregateVersion)
        {
            Weight = weight;
            Height = height;
            Color = color;
        }
    }
}
