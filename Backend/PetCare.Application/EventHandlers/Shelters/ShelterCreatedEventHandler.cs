namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterCreatedEvent.
/// </summary>
public sealed class ShelterCreatedEventHandler : IDomainEventHandler<ShelterCreatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
