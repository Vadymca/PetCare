namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
