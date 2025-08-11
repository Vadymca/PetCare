namespace PetCare.Application.EventHandlers.AdoptionApplications;

using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Handles the <see cref="AdoptionApplicationApprovedEvent"/>.
/// </summary>
public sealed class AdoptionApplicationApprovedHandler : IDomainEventHandler<AdoptionApplicationApprovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationApprovedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
