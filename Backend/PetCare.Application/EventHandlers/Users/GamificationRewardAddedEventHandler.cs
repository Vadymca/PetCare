namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles GamificationRewardAddedEvent.
/// </summary>
public sealed class GamificationRewardAddedEventHandler : IDomainEventHandler<GamificationRewardAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(GamificationRewardAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
