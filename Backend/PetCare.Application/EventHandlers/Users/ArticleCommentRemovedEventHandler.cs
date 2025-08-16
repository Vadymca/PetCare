namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
