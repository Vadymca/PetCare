namespace PetCare.Application.EventHandlers.Animals;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalStatusChangedEvent.
/// </summary>
public sealed class AnimalStatusChangedEventHandler : IDomainEventHandler<AnimalStatusChangedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalStatusChangedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
