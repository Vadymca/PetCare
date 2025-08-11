namespace PetCare.Application.EventHandlers.Animals;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;


/// <summary>
/// Handles AnimalCreatedEvent.
/// </summary>
public sealed class AnimalCreatedEventHandler : IDomainEventHandler<AnimalCreatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
