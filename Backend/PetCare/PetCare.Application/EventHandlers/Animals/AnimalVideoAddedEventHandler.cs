namespace PetCare.Application.EventHandlers.Animals;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalVideoAddedEvent.
/// </summary>
public sealed class AnimalVideoAddedEventHandler : INotificationHandler<AnimalVideoAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalVideoAddedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
