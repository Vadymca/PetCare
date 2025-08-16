namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskRewardRemovedEvent.
/// </summary>
public sealed class VolunteerTaskRewardRemovedEventHandler : INotificationHandler<VolunteerTaskRewardRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskRewardRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
