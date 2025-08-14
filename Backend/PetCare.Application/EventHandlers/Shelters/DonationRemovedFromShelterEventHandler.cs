namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles DonationRemovedFromShelterEvent.
/// </summary>
public sealed class DonationRemovedFromShelterEventHandler : IDomainEventHandler<DonationRemovedFromShelterEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(DonationRemovedFromShelterEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
