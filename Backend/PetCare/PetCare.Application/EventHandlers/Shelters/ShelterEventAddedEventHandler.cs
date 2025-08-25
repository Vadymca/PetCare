namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterEventAddedEvent.
/// </summary>
public sealed class ShelterEventAddedEventHandler : INotificationHandler<ShelterEventAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterEventAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
