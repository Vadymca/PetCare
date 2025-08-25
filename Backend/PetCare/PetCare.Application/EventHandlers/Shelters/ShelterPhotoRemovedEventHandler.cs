namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterPhotoRemovedEvent.
/// </summary>
public sealed class ShelterPhotoRemovedEventHandler : INotificationHandler<ShelterPhotoRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterPhotoRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}