namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AnimalAidRequestRemovedEvent.
/// </summary>
public sealed class AnimalAidRequestRemovedEventHandler : IDomainEventHandler<AnimalAidRequestRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(AnimalAidRequestRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
