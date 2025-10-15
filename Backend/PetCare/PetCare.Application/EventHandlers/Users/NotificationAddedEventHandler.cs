namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
