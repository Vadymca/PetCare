namespace PetCare.Application.EventHandlers.Species;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles SpecieCreatedEvent.
/// </summary>
public sealed class SpecieCreatedEventHandler : IDomainEventHandler<SpecieCreatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(SpecieCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
