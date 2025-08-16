namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserProfilePhotoChangedEvent.
/// </summary>
public sealed class UserProfilePhotoChangedEventHandler : INotificationHandler<UserProfilePhotoChangedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserProfilePhotoChangedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
