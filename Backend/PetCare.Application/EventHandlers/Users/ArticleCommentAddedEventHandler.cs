namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ArticleCommentAddedEvent.
/// </summary>
public sealed class ArticleCommentAddedEventHandler : IDomainEventHandler<ArticleCommentAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ArticleCommentAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
