namespace PetCare.Application.EventHandlers.VolunteerTasks;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskSkillRemovedEvent.
/// </summary>
public sealed class VolunteerTaskSkillRemovedEventHandler : INotificationHandler<VolunteerTaskSkillRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(VolunteerTaskSkillRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
