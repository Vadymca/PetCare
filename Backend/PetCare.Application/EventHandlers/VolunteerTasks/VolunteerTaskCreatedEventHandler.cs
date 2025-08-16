namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskCreatedEvent.
/// </summary>
public sealed class VolunteerTaskCreatedEventHandler : INotificationHandler<VolunteerTaskCreatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
