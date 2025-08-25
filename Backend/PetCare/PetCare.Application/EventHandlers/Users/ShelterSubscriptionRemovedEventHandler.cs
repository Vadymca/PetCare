namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterSubscriptionRemovedEvent.
/// </summary>
public sealed class ShelterSubscriptionRemovedEventHandler : INotificationHandler<ShelterSubscriptionRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterSubscriptionRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
