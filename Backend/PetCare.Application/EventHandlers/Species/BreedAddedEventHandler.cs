namespace PetCare.Application.EventHandlers.Species;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles BreedAddedEvent.
/// </summary>
public sealed class BreedAddedEventHandler : IDomainEventHandler<BreedAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(BreedAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
