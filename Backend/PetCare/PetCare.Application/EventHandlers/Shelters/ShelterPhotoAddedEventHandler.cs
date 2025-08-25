namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterPhotoAddedEvent.
/// </summary>
public sealed class ShelterPhotoAddedEventHandler : INotificationHandler<ShelterPhotoAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterPhotoAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}