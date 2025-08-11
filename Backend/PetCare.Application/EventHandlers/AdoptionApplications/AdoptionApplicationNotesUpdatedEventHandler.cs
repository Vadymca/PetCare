namespace PetCare.Application.EventHandlers.AdoptionApplications;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationNotesUpdatedEvent.
/// </summary>
public sealed class AdoptionApplicationNotesUpdatedEventHandler : IDomainEventHandler<AdoptionApplicationNotesUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationNotesUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
