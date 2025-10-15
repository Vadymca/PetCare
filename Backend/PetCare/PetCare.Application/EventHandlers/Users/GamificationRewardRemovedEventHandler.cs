namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles GamificationRewardRemovedEvent.
/// </summary>
public sealed class GamificationRewardRemovedEventHandler : INotificationHandler<GamificationRewardRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(GamificationRewardRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
