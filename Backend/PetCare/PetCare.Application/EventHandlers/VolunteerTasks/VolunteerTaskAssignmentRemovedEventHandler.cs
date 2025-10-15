namespace PetCare.Application.EventHandlers.VolunteerTasks;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles VolunteerTaskAssignmentRemovedEvent.
/// </summary>
public sealed class VolunteerTaskAssignmentRemovedEventHandler : INotificationHandler<VolunteerTaskAssignmentRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskAssignmentRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
