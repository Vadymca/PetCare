namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles DonationAddedEvent.
/// </summary>
public sealed class DonationAddedEventHandler : INotificationHandler<DonationAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(DonationAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
