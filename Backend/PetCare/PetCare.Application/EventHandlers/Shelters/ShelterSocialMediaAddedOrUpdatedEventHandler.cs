namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterSocialMediaAddedOrUpdatedEvent.
/// </summary>
public sealed class ShelterSocialMediaAddedOrUpdatedEventHandler : INotificationHandler<ShelterSocialMediaAddedOrUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterSocialMediaAddedOrUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}