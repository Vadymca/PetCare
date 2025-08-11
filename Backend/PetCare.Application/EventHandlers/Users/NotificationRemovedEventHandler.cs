namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles NotificationRemovedEvent.
/// </summary>
public sealed class NotificationRemovedEventHandler : IDomainEventHandler<NotificationRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(NotificationRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
