namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserLastLoginSetEvent.
/// </summary>
public sealed class UserLastLoginSetEventHandler : IDomainEventHandler<UserLastLoginSetEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(UserLastLoginSetEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
