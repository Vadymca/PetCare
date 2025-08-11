namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ArticleAddedEvent.
/// </summary>
public sealed class ArticleAddedEventHandler : IDomainEventHandler<ArticleAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ArticleAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
