namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserPasswordChangedEvent.
/// </summary>
public class UserPasswordChangedEventHandler : IDomainEventHandler<UserPasswordChangedEvent>
{
    /// <inheritdoc/>
    public Task HandleAsync(UserPasswordChangedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        return Task.CompletedTask;
    }
}
