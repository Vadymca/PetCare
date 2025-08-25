namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
