namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
