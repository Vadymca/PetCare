namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles EventRemovedEvent.
/// </summary>
public sealed class EventRemovedEventHandler : INotificationHandler<EventRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(EventRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
