namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles ShelterUpdatedEvent.
/// </summary>
public sealed class ShelterUpdatedEventHandler : IDomainEventHandler<ShelterUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
