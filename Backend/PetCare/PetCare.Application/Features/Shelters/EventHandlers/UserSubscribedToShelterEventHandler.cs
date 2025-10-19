namespace PetCare.Application.Features.Shelters.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications when a user subscribes to a shelter event.
/// </summary>
/// <remarks>This event handler currently performs no action when a user subscribes to a shelter event. It
/// is implemented to fulfill the notification handling contract and may be extended in the future to support
/// additional processing.</remarks>
public sealed class UserSubscribedToShelterEventHandler : INotificationHandler<UserSubscribedToShelterEvent>
{
   /// <summary>
   /// Handles the event when a user subscribes to a shelter. This implementation performs no action.
   /// </summary>
   /// <param name="notification">The event data containing information about the user subscription to a shelter.</param>
   /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
   /// <returns>A completed task that represents the handling of the event.</returns>
    public async Task Handle(UserSubscribedToShelterEvent notification, CancellationToken cancellationToken)
    {
        // No operation needed for this event at the moment.
        await Task.CompletedTask;
    }
}