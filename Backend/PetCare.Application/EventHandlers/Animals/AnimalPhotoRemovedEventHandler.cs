namespace PetCare.Application.EventHandlers.Animals;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalPhotoRemovedEventHandler.
/// </summary>
public sealed class AnimalPhotoRemovedEventHandler : IDomainEventHandler<AnimalPhotoRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalPhotoRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
