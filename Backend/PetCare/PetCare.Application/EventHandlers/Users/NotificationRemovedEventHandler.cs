namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
