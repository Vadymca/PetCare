namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
