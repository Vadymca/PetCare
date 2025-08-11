namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles EventAddedEvent.
/// </summary>
public sealed class EventAddedEventHandler : IDomainEventHandler<EventAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(EventAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
