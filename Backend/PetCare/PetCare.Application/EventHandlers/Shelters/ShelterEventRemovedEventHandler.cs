namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterEventRemovedEvent.
/// </summary>
public sealed class ShelterEventRemovedEventHandler : INotificationHandler<ShelterEventRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ShelterEventRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
