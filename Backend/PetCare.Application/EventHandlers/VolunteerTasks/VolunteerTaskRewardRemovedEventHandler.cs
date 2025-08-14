namespace PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles VolunteerTaskRewardRemovedEvent.
/// </summary>
public sealed class VolunteerTaskRewardRemovedEventHandler : IDomainEventHandler<VolunteerTaskRewardRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(VolunteerTaskRewardRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
