namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterSubscriptionAddedEvent.
/// </summary>
public sealed class ShelterSubscriptionAddedEventHandler : IDomainEventHandler<ShelterSubscriptionAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterSubscriptionAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
