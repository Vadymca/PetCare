namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles SuccessStoryRemovedEvent.
/// </summary>
public sealed class SuccessStoryRemovedEventHandler : IDomainEventHandler<SuccessStoryRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(SuccessStoryRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
