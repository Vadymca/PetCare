namespace PetCare.Application.EventHandlers.VolunteerTasks;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
