namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserPointsAddedEvent.
/// </summary>
public sealed class UserPointsAddedEventHandler : IDomainEventHandler<UserPointsAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(UserPointsAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
