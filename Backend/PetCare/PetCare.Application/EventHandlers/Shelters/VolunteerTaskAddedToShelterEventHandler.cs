namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskAddedToShelterEvent.
/// </summary>
public sealed class VolunteerTaskAddedToShelterEventHandler : INotificationHandler<VolunteerTaskAddedToShelterEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskAddedToShelterEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
