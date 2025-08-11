namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskInfoUpdatedEvent.
/// </summary>
public sealed class VolunteerTaskInfoUpdatedEventHandler : IDomainEventHandler<VolunteerTaskInfoUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskInfoUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
