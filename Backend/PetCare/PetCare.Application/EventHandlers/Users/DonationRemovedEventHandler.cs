namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles DonationRemovedEvent.
/// </summary>
public sealed class DonationRemovedEventHandler : INotificationHandler<DonationRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(DonationRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
