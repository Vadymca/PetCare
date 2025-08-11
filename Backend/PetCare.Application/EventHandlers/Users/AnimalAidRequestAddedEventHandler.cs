namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AnimalAidRequestAddedEvent.
/// </summary>
public sealed class AnimalAidRequestAddedEventHandler : IDomainEventHandler<AnimalAidRequestAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalAidRequestAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
