namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles SuccessStoryAddedEvent.
/// </summary>
public sealed class SuccessStoryAddedEventHandler : INotificationHandler<SuccessStoryAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(SuccessStoryAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
