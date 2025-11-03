namespace PetCare.Application.EventHandlers.Guardianship;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications when a guardianship process has been completed.
/// </summary>
/// <remarks>This event handler is typically used within a MediatR pipeline to respond to the completion
/// of a guardianship event. It implements the INotificationHandler interface for GuardianshipCompletedEvent,
/// allowing it to be registered and invoked by the application's event handling infrastructure.</remarks>
public sealed class GuardianshipCompletedEventHandler : INotificationHandler<GuardianshipCompletedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(GuardianshipCompletedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
