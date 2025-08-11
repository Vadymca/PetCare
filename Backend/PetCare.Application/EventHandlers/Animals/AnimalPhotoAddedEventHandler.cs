namespace PetCare.Application.EventHandlers.Animals;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalPhotoAddedEventHandler.
/// </summary>
public sealed class AnimalPhotoAddedEventHandler : IDomainEventHandler<AnimalPhotoAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalPhotoAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
