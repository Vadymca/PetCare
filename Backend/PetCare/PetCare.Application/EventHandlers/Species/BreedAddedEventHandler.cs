namespace PetCare.Application.EventHandlers.Species;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles BreedAddedEvent.
/// </summary>
public sealed class BreedAddedEventHandler : INotificationHandler<BreedAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(BreedAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
