namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles LostPetRemovedEvent.
/// </summary>
public sealed class LostPetRemovedEventHandler : IDomainEventHandler<LostPetRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(LostPetRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
