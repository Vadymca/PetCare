namespace PetCare.Application.EventHandlers.AdoptionApplications;

using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Handles the <see cref="AdoptionApplicationCreatedEvent"/>.
/// </summary>
public sealed class AdoptionApplicationCreatedHandler : IDomainEventHandler<AdoptionApplicationCreatedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
