namespace PetCare.Application.EventHandlers.Animals;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalPhotoAddedEventHandler.
/// </summary>
public sealed class AnimalPhotoAddedEventHandler : INotificationHandler<AnimalPhotoAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalPhotoAddedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
