namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles BreedAddedEvent.
/// </summary>
public sealed class UserPointsDeductedEventHandler : INotificationHandler<UserPointsDeductedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserPointsDeductedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
