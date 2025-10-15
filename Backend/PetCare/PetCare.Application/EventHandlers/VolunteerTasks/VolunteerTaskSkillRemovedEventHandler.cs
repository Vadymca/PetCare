namespace PetCare.Application.EventHandlers.VolunteerTasks;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
