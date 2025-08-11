namespace PetCare.Application.EventHandlers.Animals;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalVideoAddedEvent.
/// </summary>
public sealed class AnimalVideoAddedEventHandler : IDomainEventHandler<AnimalVideoAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalVideoAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
