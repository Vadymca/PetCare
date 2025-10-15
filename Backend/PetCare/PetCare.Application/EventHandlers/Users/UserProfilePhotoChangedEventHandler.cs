namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
