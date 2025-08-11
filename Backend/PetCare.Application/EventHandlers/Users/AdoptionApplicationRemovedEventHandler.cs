namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationRemovedEvent.
/// </summary>
public sealed class AdoptionApplicationRemovedEventHandler : IDomainEventHandler<AdoptionApplicationRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AdoptionApplicationRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
