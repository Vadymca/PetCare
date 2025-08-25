namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles LostPetRemovedEvent.
/// </summary>
public sealed class LostPetRemovedEventHandler : INotificationHandler<LostPetRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(LostPetRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
