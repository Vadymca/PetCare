namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterPhotoRemovedEvent.
/// </summary>
public sealed class ShelterPhotoRemovedEventHandler : IDomainEventHandler<ShelterPhotoRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterPhotoRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}