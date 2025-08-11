namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ArticleCommentRemovedEvent.
/// </summary>
public sealed class ArticleCommentRemovedEventHandler : IDomainEventHandler<ArticleCommentRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(ArticleCommentRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
