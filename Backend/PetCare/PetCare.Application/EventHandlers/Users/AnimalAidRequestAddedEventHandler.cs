namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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
