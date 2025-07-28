namespace PetCare.Domain.Common
{
    /// <summary>
    /// Base class for aggregate roots with domain event support
    /// </summary>
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        private int _version = 0;

        protected AggregateRoot() : base() { }
        protected AggregateRoot(Guid id) : base(id) { }

        /// <summary>
        /// Current version of the aggregate (for optimistic concurrency)
        /// </summary>
        public int Version => _version;

        /// <summary>
        /// Domain events that have been raised by this aggregate
        /// </summary>
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Add a domain event to be published
        /// </summary>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Remove a specific domain event
        /// </summary>
        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        /// <summary>
        /// Clear all domain events
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        /// <summary>
        /// Increment the aggregate version
        /// </summary>
        protected void IncrementVersion()
        {
            _version++;
            UpdateTimestamp();
        }

        /// <summary>
        /// Mark the aggregate as modified and increment version
        /// </summary>
        protected void MarkAsModified()
        {
            IncrementVersion();
        }
    }
}
