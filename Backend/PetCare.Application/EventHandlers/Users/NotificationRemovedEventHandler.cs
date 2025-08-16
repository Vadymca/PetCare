namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles NotificationRemovedEvent.
/// </summary>
public sealed class NotificationRemovedEventHandler : INotificationHandler<NotificationRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(NotificationRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
