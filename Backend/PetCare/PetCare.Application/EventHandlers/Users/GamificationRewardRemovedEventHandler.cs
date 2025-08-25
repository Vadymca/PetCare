namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
