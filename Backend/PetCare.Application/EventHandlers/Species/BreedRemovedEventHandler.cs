namespace PetCare.Application.EventHandlers.Species;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles BreedRemovedEvent.
/// </summary>
public sealed class BreedRemovedEventHandler : INotificationHandler<BreedRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(BreedRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
