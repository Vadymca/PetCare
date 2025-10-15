namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
