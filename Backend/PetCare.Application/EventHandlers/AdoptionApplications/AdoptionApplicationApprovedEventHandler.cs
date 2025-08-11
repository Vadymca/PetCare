namespace PetCare.Application.EventHandlers.AdoptionApplications;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationApprovedEvent.
/// </summary>
public sealed class AdoptionApplicationApprovedEventHandler : IDomainEventHandler<AdoptionApplicationApprovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationApprovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // логіка
        await Task.CompletedTask;
    }
}
