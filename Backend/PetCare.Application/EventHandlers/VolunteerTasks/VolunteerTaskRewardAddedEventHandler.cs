namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskRewardAddedEvent.
/// </summary>

public sealed class VolunteerTaskRewardAddedEventHandler : INotificationHandler<VolunteerTaskRewardAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskRewardAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
