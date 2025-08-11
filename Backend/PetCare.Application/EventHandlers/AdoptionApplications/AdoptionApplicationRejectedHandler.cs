namespace PetCare.Application.EventHandlers.AdoptionApplications;

using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Handles the event raised when an adoption application is rejected.
/// </summary>
public sealed class AdoptionApplicationRejectedHandler : IDomainEventHandler<AdoptionApplicationRejectedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationRejectedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
