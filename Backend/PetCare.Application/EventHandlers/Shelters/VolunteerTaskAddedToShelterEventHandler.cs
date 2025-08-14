namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskAddedToShelterEvent.
/// </summary>
public sealed class VolunteerTaskAddedToShelterEventHandler : IDomainEventHandler<VolunteerTaskAddedToShelterEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskAddedToShelterEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
