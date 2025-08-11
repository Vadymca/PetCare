namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AnimalAddedToShelterEvent.
/// </summary>
public sealed class AnimalAddedToShelterEventHandler : IDomainEventHandler<AnimalAddedToShelterEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalAddedToShelterEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
