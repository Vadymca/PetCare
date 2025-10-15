namespace PetCare.Application.EventHandlers.Shelters;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
