using PetCare.Domain.Common;

namespace PetCare.Domain.Events
{
    public sealed class ShelterCreatedEvent : DomainEventBase
    {
        public string Name { get; }
        public string Address { get; }
        public Guid? ManagerId { get; }
        public int Capacity { get; }

        public ShelterCreatedEvent(Guid aggregateId, int aggregateVersion, string name, string address, Guid? managerId, int capacity)
            : base(aggregateId, aggregateVersion)
        {
            Name = name;
            Address = address;
            ManagerId = managerId;
            Capacity = capacity;
        }
    }

    public sealed class ShelterContactInfoUpdatedEvent : DomainEventBase
    {
        public string? NewPhone { get; }
        public string? NewEmail { get; }

        public ShelterContactInfoUpdatedEvent(Guid aggregateId, int aggregateVersion, string? newPhone, string? newEmail)
            : base(aggregateId, aggregateVersion)
        {
            NewPhone = newPhone;
            NewEmail = newEmail;
        }
    }

    public sealed class ShelterCapacityChangedEvent : DomainEventBase
    {
        public int PreviousCapacity { get; }
        public int NewCapacity { get; }

        public ShelterCapacityChangedEvent(Guid aggregateId, int aggregateVersion, int previousCapacity, int newCapacity)
            : base(aggregateId, aggregateVersion)
        {
            PreviousCapacity = previousCapacity;
            NewCapacity = newCapacity;
        }
    }

    public sealed class ShelterOccupancyChangedEvent : DomainEventBase
    {
        public int PreviousOccupancy { get; }
        public int NewOccupancy { get; }
        public bool IsAtCapacity { get; }

        public ShelterOccupancyChangedEvent(Guid aggregateId, int aggregateVersion, int previousOccupancy, int newOccupancy, bool isAtCapacity)
            : base(aggregateId, aggregateVersion)
        {
            PreviousOccupancy = previousOccupancy;
            NewOccupancy = newOccupancy;
            IsAtCapacity = isAtCapacity;
        }
    }

    public sealed class ShelterManagerChangedEvent : DomainEventBase
    {
        public Guid? PreviousManagerId { get; }
        public Guid? NewManagerId { get; }

        public ShelterManagerChangedEvent(Guid aggregateId, int aggregateVersion, Guid? previousManagerId, Guid? newManagerId)
            : base(aggregateId, aggregateVersion)
        {
            PreviousManagerId = previousManagerId;
            NewManagerId = newManagerId;
        }
    }

    public sealed class ShelterWorkingHoursUpdatedEvent : DomainEventBase
    {
        public string? NewWorkingHours { get; }

        public ShelterWorkingHoursUpdatedEvent(Guid aggregateId, int aggregateVersion, string? newWorkingHours)
            : base(aggregateId, aggregateVersion)
        {
            NewWorkingHours = newWorkingHours;
        }
    }
}
