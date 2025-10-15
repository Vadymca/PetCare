namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
