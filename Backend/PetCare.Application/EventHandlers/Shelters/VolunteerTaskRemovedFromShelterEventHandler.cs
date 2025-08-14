namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskRemovedFromShelterEvent.
/// </summary>
public sealed class VolunteerTaskRemovedFromShelterEventHandler : IDomainEventHandler<VolunteerTaskRemovedFromShelterEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskRemovedFromShelterEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
