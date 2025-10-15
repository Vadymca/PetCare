namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles ArticleAddedEvent.
/// </summary>
public sealed class ArticleAddedEventHandler : INotificationHandler<ArticleAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(ArticleAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
