namespace PetCare.Domain.Common
{
    /// <summary>
    /// Interface for dispatching domain events
    /// </summary>
    public interface IDomainEventDispatcher
    {
        /// <summary>
        /// Dispatch a single domain event
        /// </summary>
        Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatch multiple domain events
        /// </summary>
        Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
    }
}
