namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles SuccessStoryRemovedEvent.
/// </summary>
public sealed class SuccessStoryRemovedEventHandler : INotificationHandler<SuccessStoryRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(SuccessStoryRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
