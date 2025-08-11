namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskCreatedEvent.
/// </summary>
public sealed class VolunteerTaskCreatedEventHandler : IDomainEventHandler<VolunteerTaskCreatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
