namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserPointsAddedEvent.
/// </summary>
public sealed class UserPointsAddedEventHandler : INotificationHandler<UserPointsAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserPointsAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
