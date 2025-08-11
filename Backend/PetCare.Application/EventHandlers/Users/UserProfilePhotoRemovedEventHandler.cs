namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserProfilePhotoRemovedEvent.
/// </summary>
public sealed class UserProfilePhotoRemovedEventHandler : IDomainEventHandler<UserProfilePhotoRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(UserProfilePhotoRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
