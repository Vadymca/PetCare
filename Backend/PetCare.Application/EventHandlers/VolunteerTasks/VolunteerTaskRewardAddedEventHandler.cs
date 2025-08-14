namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskRewardAddedEvent.
/// </summary>

public sealed class VolunteerTaskRewardAddedEventHandler : IDomainEventHandler<VolunteerTaskRewardAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskRewardAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
