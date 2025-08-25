namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserProfilePhotoRemovedEvent.
/// </summary>
public sealed class UserProfilePhotoRemovedEventHandler : INotificationHandler<UserProfilePhotoRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserProfilePhotoRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
