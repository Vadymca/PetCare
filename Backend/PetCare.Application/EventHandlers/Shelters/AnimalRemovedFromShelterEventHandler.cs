namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AnimalRemovedFromShelterEvent.
/// </summary>
public sealed class AnimalRemovedFromShelterEventHandler : IDomainEventHandler<AnimalRemovedFromShelterEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalRemovedFromShelterEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
