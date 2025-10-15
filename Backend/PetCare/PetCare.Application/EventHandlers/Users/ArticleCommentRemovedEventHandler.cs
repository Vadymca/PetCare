namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ArticleCommentRemovedEvent.
/// </summary>
public sealed class ArticleCommentRemovedEventHandler : INotificationHandler<ArticleCommentRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ArticleCommentRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
