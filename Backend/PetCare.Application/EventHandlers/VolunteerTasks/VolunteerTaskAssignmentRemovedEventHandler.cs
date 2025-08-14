namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskAssignmentRemovedEvent.
/// </summary>
public sealed class VolunteerTaskAssignmentRemovedEventHandler : IDomainEventHandler<VolunteerTaskAssignmentRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskAssignmentRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
