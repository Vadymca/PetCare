namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles LostPetAddedEvent.
/// </summary>
public sealed class LostPetAddedEventHandler : INotificationHandler<LostPetAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(LostPetAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
