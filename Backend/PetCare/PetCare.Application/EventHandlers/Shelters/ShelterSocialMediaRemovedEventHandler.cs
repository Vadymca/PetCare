namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterSocialMediaRemovedEvent.
/// </summary>
public sealed class ShelterSocialMediaRemovedEventHandler : INotificationHandler<ShelterSocialMediaRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterSocialMediaRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
