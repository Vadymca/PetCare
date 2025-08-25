namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterUpdatedEvent.
/// </summary>
public sealed class ShelterUpdatedEventHandler : INotificationHandler<ShelterUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
