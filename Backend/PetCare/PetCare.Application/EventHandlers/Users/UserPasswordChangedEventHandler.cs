namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserPasswordChangedEvent.
/// </summary>
public class UserPasswordChangedEventHandler : INotificationHandler<UserPasswordChangedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserPasswordChangedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
