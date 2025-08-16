namespace PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Represents a dispatcher for domain events.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches all domain events asynchronously.
    /// </summary>
    /// <param name="events">The collection of domain events.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DispatchAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default);
}
