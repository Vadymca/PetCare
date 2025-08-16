namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AnimalAidRequestRemovedEvent.
/// </summary>
public sealed class AnimalAidRequestRemovedEventHandler : INotificationHandler<AnimalAidRequestRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalAidRequestRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
