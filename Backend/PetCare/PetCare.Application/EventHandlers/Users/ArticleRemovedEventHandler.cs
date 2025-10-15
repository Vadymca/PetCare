namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ArticleRemovedEvent.
/// </summary>
public sealed class ArticleRemovedEventHandler : INotificationHandler<ArticleRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ArticleRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
