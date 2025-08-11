namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserCreatedEvent.
/// </summary>
public sealed class UserCreatedEventHandler : IDomainEventHandler<UserCreatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(UserCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
