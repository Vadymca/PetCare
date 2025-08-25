namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserLastLoginSetEvent.
/// </summary>
public sealed class UserLastLoginSetEventHandler : INotificationHandler<UserLastLoginSetEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserLastLoginSetEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
