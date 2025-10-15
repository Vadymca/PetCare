namespace PetCare.Application.EventHandlers.VolunteerTasks;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
