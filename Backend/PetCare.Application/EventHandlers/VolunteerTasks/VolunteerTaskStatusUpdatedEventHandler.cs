namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskStatusUpdatedEvent.
/// </summary>
public sealed class VolunteerTaskStatusUpdatedEventHandler : INotificationHandler<VolunteerTaskStatusUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskStatusUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
