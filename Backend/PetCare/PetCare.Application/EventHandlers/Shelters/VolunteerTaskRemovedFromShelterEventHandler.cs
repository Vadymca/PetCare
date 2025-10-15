namespace PetCare.Application.EventHandlers.Shelters;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles VolunteerTaskRemovedFromShelterEvent.
/// </summary>
public sealed class VolunteerTaskRemovedFromShelterEventHandler : INotificationHandler<VolunteerTaskRemovedFromShelterEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskRemovedFromShelterEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
