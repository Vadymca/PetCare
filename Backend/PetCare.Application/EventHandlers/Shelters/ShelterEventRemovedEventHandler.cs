namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterEventRemovedEvent.
/// </summary>
public sealed class ShelterEventRemovedEventHandler : IDomainEventHandler<ShelterEventRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterEventRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
