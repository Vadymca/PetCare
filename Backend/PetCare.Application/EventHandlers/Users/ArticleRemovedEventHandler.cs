namespace PetCare.Application.EventHandlers.Users;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles ArticleRemovedEvent.
/// </summary>
public sealed class ArticleRemovedEventHandler : IDomainEventHandler<ArticleRemovedEvent>
{
    public async Task HandleAsync(ArticleRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
