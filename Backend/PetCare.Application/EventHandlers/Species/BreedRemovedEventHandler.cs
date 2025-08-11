namespace PetCare.Application.EventHandlers.Species;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles BreedRemovedEvent.
/// </summary>
public sealed class BreedRemovedEventHandler : IDomainEventHandler<BreedRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(BreedRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
