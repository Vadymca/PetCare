namespace PetCare.Application.EventHandlers.Guardianship;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications when a guardianship donation is added.
/// </summary>
/// <remarks>This event handler processes the <see cref="GuardianshipDonationAddedEvent"/> using the MediatR
/// notification pattern. It is typically used to trigger domain logic or side effects in response to new guardianship
/// donations. This class is sealed and cannot be inherited.</remarks>
public sealed class GuardianshipDonationAddedEventHandler : INotificationHandler<GuardianshipDonationAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(GuardianshipDonationAddedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
