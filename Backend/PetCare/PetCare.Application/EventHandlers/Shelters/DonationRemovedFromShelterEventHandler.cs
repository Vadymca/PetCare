namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
