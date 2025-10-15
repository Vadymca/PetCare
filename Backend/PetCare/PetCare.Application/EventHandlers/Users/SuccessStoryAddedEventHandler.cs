namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
