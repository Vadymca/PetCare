namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AnimalAidRequestAddedEvent.
/// </summary>
public sealed class AnimalAidRequestAddedEventHandler : INotificationHandler<AnimalAidRequestAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AnimalAidRequestAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
