namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
