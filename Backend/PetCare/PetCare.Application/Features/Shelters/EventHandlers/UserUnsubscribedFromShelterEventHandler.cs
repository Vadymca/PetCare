namespace PetCare.Application.Features.Shelters.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications when a user unsubscribes from a shelter.
/// </summary>
/// <remarks>This event handler is invoked when a user unsubscribes from a shelter, allowing the
/// application to respond to the event. Currently, no action is performed when the event is handled. This class is
/// typically used within a notification pipeline to observe user unsubscription events.</remarks>
public sealed class UserUnsubscribedFromShelterEventHandler : INotificationHandler<UserUnsubscribedFromShelterEvent>
{
    /// <summary>
    /// Handles the event when a user unsubscribes from a shelter. This method is invoked in response to a user
    /// unsubscription notification.
    /// </summary>
    /// <param name="notification">The event data containing information about the user who unsubscribed from the shelter.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    public async Task Handle(UserUnsubscribedFromShelterEvent notification, CancellationToken cancellationToken)
    {
        // No operation needed for this event at the moment.
        await Task.CompletedTask;
    }
}
