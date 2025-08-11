namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskSkillRemovedEvent.
/// </summary>
public sealed class VolunteerTaskSkillRemovedEventHandler : IDomainEventHandler<VolunteerTaskSkillRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskSkillRemovedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
