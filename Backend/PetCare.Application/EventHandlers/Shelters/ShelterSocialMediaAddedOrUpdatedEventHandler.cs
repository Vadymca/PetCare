namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterSocialMediaAddedOrUpdatedEvent.
/// </summary>
public sealed class ShelterSocialMediaAddedOrUpdatedEventHandler : IDomainEventHandler<ShelterSocialMediaAddedOrUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterSocialMediaAddedOrUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}