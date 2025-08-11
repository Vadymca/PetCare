namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles BreedAddedEvent.
/// </summary>
public sealed class UserPointsDeductedEventHandler : IDomainEventHandler<UserPointsDeductedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(UserPointsDeductedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
