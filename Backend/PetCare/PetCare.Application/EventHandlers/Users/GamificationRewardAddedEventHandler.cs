namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles GamificationRewardAddedEvent.
/// </summary>
public sealed class GamificationRewardAddedEventHandler : INotificationHandler<GamificationRewardAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(GamificationRewardAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
