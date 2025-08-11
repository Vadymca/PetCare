namespace PetCare.Application.EventHandlers.AdoptionApplications;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationRejectedEvent.
/// </summary>
public sealed class AdoptionApplicationRejectedEventHandler : IDomainEventHandler<AdoptionApplicationRejectedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationRejectedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
