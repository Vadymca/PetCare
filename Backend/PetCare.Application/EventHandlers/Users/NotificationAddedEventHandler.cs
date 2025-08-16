namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles NotificationAddedEvent.
/// </summary>
public sealed class NotificationAddedEventHandler : INotificationHandler<NotificationAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(NotificationAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
