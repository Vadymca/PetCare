namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskInfoUpdatedEvent.
/// </summary>
public sealed class VolunteerTaskInfoUpdatedEventHandler : INotificationHandler<VolunteerTaskInfoUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskInfoUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
