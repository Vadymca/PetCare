namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles DonationRemovedEvent.
/// </summary>
public sealed class DonationRemovedEventHandler : IDomainEventHandler<DonationRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(DonationRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
