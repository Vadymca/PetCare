namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationAddedEvent.
/// </summary>
public sealed class AdoptionApplicationAddedEventHandler : IDomainEventHandler<AdoptionApplicationAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
