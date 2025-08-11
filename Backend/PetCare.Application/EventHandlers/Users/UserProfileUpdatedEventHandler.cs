namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserProfileUpdatedEvent.
/// </summary>
public sealed class UserProfileUpdatedEventHandler : IDomainEventHandler<UserProfileUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(UserProfileUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
