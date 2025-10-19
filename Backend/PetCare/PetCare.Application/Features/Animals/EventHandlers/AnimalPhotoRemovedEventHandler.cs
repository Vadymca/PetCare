namespace PetCare.Application.Features.Animals.EventHandlers;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalPhotoRemovedEventHandler.
/// </summary>
public sealed class AnimalPhotoRemovedEventHandler : INotificationHandler<AnimalPhotoRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalPhotoRemovedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
