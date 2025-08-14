namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles DonationAddedToShelterEvent.
/// </summary>
public sealed class DonationAddedToShelterEventHandler : IDomainEventHandler<DonationAddedToShelterEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(DonationAddedToShelterEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
