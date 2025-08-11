namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles LostPetAddedEvent.
/// </summary>
public sealed class LostPetAddedEventHandler : IDomainEventHandler<LostPetAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(LostPetAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
