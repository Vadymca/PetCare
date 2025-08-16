namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserProfileUpdatedEvent.
/// </summary>
public sealed class UserProfileUpdatedEventHandler : INotificationHandler<UserProfileUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserProfileUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
