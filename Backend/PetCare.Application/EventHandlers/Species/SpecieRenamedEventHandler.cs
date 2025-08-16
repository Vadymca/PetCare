namespace PetCare.Application.EventHandlers.Species;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles SpecieRenamedEvent.
/// </summary>
public sealed class SpecieRenamedEventHandler : INotificationHandler<SpecieRenamedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(SpecieRenamedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
