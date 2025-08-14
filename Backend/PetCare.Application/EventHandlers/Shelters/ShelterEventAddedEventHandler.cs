namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ShelterEventAddedEvent.
/// </summary>
public sealed class ShelterEventAddedEventHandler : IDomainEventHandler<ShelterEventAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ShelterEventAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
