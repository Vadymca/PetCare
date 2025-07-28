namespace PetCare.Domain.Common
{
    /// <summary>
    /// Base class for all domain events
    /// </summary>
    public abstract class DomainEventBase : IDomainEvent
    {
        public Guid EventId { get; }
        public DateTime OccurredOn { get; }
        public Guid AggregateId { get; }
        public int AggregateVersion { get; }

        protected DomainEventBase(Guid aggregateId, int aggregateVersion)
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            AggregateId = aggregateId;
            AggregateVersion = aggregateVersion;
        }
    }
}
