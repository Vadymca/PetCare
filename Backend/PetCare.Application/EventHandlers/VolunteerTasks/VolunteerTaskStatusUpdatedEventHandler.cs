namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskStatusUpdatedEvent.
/// </summary>
public sealed class VolunteerTaskStatusUpdatedEventHandler : IDomainEventHandler<VolunteerTaskStatusUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskStatusUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
