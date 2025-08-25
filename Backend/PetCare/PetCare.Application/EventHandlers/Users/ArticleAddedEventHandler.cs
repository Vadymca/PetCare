namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
