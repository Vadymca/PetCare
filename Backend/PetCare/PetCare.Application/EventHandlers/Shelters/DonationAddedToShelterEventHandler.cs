namespace PetCare.Application.EventHandlers.Shelters;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles DonationAddedToShelterEvent.
/// </summary>
public sealed class DonationAddedToShelterEventHandler : INotificationHandler<DonationAddedToShelterEvent>
{
    /// <inheritdoc/>
    public async Task Handle(DonationAddedToShelterEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
