namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles UserProfilePhotoChangedEvent.
/// </summary>
public sealed class UserProfilePhotoChangedEventHandler : IDomainEventHandler<UserProfilePhotoChangedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(UserProfilePhotoChangedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
