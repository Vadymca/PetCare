namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles DonationAddedEvent.
/// </summary>
public sealed class DonationAddedEventHandler : IDomainEventHandler<DonationAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(DonationAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
