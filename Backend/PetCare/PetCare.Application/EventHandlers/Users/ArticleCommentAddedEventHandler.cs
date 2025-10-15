namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
