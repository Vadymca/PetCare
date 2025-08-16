namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterSubscriptionUpdatedEvent.
/// </summary>
public sealed class ShelterSubscriptionUpdatedEventHandler : INotificationHandler<ShelterSubscriptionUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterSubscriptionUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
