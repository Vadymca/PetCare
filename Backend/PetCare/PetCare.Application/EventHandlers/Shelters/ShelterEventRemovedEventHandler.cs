namespace PetCare.Application.EventHandlers.Shelters;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
