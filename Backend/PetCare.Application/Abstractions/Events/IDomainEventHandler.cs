namespace PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Represents a domain event handler for any domain event.
/// </summary>
/// <typeparam name="TEvent">The type of domain event.</typeparam>
public interface IDomainEventHandler<in TEvent>
    where TEvent : DomainEvent
{
    /// <summary>
    /// Handles a specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
