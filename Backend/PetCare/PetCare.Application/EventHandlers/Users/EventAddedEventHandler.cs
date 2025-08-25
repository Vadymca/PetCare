namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles EventAddedEvent.
/// </summary>
public sealed class EventAddedEventHandler : INotificationHandler<EventAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(EventAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
