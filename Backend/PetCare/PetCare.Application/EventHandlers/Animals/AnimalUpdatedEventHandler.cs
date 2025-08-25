namespace PetCare.Application.EventHandlers.Animals;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalStatusChangedEvent.
/// </summary>
public sealed class AnimalStatusChangedEventHandler : INotificationHandler<AnimalStatusChangedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
