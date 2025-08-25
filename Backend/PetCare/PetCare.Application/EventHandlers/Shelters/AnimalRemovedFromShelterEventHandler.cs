namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AnimalRemovedFromShelterEvent.
/// </summary>
public sealed class AnimalRemovedFromShelterEventHandler : INotificationHandler<AnimalRemovedFromShelterEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalRemovedFromShelterEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
