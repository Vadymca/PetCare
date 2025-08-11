namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterSubscriptionRemovedEvent.
/// </summary>
public sealed class ShelterSubscriptionRemovedEventHandler : IDomainEventHandler<ShelterSubscriptionRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterSubscriptionRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
