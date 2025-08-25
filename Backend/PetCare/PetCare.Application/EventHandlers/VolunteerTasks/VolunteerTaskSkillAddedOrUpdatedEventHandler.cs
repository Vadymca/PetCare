namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
