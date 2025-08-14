namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskAssignmentAddedEvent.
/// </summary>
public sealed class VolunteerTaskAssignmentAddedEventHandler : IDomainEventHandler<VolunteerTaskAssignmentAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskAssignmentAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
