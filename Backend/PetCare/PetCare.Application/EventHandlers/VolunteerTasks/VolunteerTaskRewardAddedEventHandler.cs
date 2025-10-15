namespace PetCare.Application.EventHandlers.VolunteerTasks;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
