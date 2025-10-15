namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
