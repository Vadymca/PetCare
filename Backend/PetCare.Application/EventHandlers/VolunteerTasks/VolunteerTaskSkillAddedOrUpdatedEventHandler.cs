namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskSkillAddedOrUpdatedEvent.
/// </summary>
public sealed class VolunteerTaskSkillAddedOrUpdatedEventHandler : IDomainEventHandler<VolunteerTaskSkillAddedOrUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskSkillAddedOrUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
