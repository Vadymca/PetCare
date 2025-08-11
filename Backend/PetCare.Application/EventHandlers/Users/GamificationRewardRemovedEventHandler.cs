namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles GamificationRewardRemovedEvent.
/// </summary>
public sealed class GamificationRewardRemovedEventHandler : IDomainEventHandler<GamificationRewardRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(GamificationRewardRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
