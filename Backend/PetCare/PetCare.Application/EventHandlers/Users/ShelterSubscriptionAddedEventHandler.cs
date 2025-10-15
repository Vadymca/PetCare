namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterSubscriptionAddedEvent.
/// </summary>
public sealed class ShelterSubscriptionAddedEventHandler : INotificationHandler<ShelterSubscriptionAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterSubscriptionAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
