namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ArticleCommentAddedEvent.
/// </summary>
public sealed class ArticleCommentAddedEventHandler : INotificationHandler<ArticleCommentAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ArticleCommentAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
