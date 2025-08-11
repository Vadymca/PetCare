namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles NotificationAddedEvent.
/// </summary>
public sealed class NotificationAddedEventHandler : IDomainEventHandler<NotificationAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(NotificationAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
