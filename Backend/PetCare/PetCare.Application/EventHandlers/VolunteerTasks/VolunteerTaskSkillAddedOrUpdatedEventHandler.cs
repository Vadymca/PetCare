namespace PetCare.Application.EventHandlers.VolunteerTasks;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles VolunteerTaskSkillAddedOrUpdatedEvent.
/// </summary>
public sealed class VolunteerTaskSkillAddedOrUpdatedEventHandler : INotificationHandler<VolunteerTaskSkillAddedOrUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskSkillAddedOrUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
