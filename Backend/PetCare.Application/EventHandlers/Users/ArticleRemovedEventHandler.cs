namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
