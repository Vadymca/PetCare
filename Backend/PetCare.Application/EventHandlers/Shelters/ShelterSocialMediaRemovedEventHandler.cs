namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterSocialMediaRemovedEvent.
/// </summary>
public sealed class ShelterSocialMediaRemovedEventHandler : IDomainEventHandler<ShelterSocialMediaRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterSocialMediaRemovedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
