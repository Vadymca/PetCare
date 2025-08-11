namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles EventRemovedEvent.
/// </summary>
public sealed class EventRemovedEventHandler : IDomainEventHandler<EventRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(EventRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
