namespace PetCare.Application.Common;
using PetCare.Domain.Events;

/// <summary>
/// Represents a domain event handler for any domain event.
/// </summary>
public interface IDomainEventHandler
{
    /// <summary>
    /// Handles a specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Handle(DomainEvent domainEvent, CancellationToken cancellationToken);
}
