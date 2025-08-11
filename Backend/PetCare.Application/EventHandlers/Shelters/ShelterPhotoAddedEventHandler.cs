namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterPhotoAddedEvent.
/// </summary>
public sealed class ShelterPhotoAddedEventHandler : IDomainEventHandler<ShelterPhotoAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterPhotoAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}