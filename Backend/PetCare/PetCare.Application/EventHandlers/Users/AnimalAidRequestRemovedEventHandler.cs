namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
