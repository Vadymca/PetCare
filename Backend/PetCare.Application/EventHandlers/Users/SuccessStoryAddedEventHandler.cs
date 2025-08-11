namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles SuccessStoryAddedEvent.
/// </summary>
public sealed class SuccessStoryAddedEventHandler : IDomainEventHandler<SuccessStoryAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(SuccessStoryAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
