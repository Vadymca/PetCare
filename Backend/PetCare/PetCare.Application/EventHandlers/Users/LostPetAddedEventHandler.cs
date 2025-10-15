namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
