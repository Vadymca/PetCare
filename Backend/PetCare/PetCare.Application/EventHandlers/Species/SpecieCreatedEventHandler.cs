namespace PetCare.Application.EventHandlers.Species;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles SpecieCreatedEvent.
/// </summary>
public sealed class SpecieCreatedEventHandler : INotificationHandler<SpecieCreatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(SpecieCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
