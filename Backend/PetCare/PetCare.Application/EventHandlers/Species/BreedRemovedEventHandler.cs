namespace PetCare.Application.EventHandlers.Species;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
