namespace PetCare.Application.EventHandlers.VolunteerTasks;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
