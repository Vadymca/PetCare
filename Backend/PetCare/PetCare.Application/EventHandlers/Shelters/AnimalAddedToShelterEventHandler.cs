namespace PetCare.Application.EventHandlers.Shelters;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
