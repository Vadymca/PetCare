namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterSubscriptionUpdatedEvent.
/// </summary>
public sealed class ShelterSubscriptionUpdatedEventHandler : IDomainEventHandler<ShelterSubscriptionUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterSubscriptionUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
