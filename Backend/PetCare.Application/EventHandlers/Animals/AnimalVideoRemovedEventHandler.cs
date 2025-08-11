namespace PetCare.Application.EventHandlers.Animals;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalVideoRemovedEventHandler.
/// </summary>
public sealed class AnimalVideoRemovedEventHandler : IDomainEventHandler<AnimalVideoRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalVideoRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
