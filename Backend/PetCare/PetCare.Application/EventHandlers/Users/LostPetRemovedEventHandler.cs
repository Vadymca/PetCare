namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
