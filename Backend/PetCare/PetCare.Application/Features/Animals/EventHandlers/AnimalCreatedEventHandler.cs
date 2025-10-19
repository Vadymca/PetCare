namespace PetCare.Application.Features.Animals.EventHandlers;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalCreatedEvent.
/// </summary>
public sealed class AnimalCreatedEventHandler : INotificationHandler<AnimalCreatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalCreatedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
