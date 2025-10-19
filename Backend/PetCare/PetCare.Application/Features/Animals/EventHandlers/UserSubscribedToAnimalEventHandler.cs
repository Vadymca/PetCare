namespace PetCare.Application.Features.Animals.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications when a user subscribes to an animal event.
/// </summary>
/// <remarks>This handler is typically used within a MediatR pipeline to process user subscription events. It is
/// sealed to prevent inheritance and ensure consistent event handling behavior.</remarks>
public sealed class UserSubscribedToAnimalEventHandler : INotificationHandler<UserSubscribedToAnimalEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserSubscribedToAnimalEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
