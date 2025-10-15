namespace PetCare.Application.EventHandlers.Shelters;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles DonationRemovedFromShelterEvent.
/// </summary>
public sealed class DonationRemovedFromShelterEventHandler : INotificationHandler<DonationRemovedFromShelterEvent>
{
    /// <inheritdoc/>
    public async Task Handle(DonationRemovedFromShelterEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
