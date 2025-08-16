namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskAssignmentAddedEvent.
/// </summary>
public sealed class VolunteerTaskAssignmentAddedEventHandler : INotificationHandler<VolunteerTaskAssignmentAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskAssignmentAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
