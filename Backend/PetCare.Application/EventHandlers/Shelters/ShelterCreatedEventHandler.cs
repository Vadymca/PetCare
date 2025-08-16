namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterCreatedEvent.
/// </summary>
public sealed class ShelterCreatedEventHandler : INotificationHandler<ShelterCreatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
