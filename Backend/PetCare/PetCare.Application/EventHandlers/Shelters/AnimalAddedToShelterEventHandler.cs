namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AnimalAddedToShelterEvent.
/// </summary>
public sealed class AnimalAddedToShelterEventHandler : INotificationHandler<AnimalAddedToShelterEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalAddedToShelterEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
