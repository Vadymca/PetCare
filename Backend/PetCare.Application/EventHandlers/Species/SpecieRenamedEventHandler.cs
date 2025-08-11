namespace PetCare.Application.EventHandlers.Species;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles SpecieRenamedEvent.
/// </summary>
public sealed class SpecieRenamedEventHandler : IDomainEventHandler<SpecieRenamedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(SpecieRenamedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
