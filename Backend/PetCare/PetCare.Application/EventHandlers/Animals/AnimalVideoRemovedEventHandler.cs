namespace PetCare.Application.EventHandlers.Animals;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalVideoRemovedEventHandler.
/// </summary>
public sealed class AnimalVideoRemovedEventHandler : INotificationHandler<AnimalVideoRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalVideoRemovedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
