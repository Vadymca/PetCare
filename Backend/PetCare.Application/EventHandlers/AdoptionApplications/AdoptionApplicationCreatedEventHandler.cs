namespace PetCare.Application.EventHandlers.AdoptionApplications;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationCreatedEvent.
/// </summary>
public sealed class AdoptionApplicationCreatedEventHandler : IDomainEventHandler<AdoptionApplicationCreatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
