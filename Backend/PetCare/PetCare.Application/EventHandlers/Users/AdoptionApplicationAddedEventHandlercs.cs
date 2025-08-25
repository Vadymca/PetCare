namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationAddedEvent.
/// </summary>
public sealed class AdoptionApplicationAddedEventHandler : INotificationHandler<AdoptionApplicationAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AdoptionApplicationAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
