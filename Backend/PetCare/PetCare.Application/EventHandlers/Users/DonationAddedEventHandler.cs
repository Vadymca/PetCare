namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
