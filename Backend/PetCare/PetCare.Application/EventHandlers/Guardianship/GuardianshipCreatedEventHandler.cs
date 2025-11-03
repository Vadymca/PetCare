namespace PetCare.Application.EventHandlers.Guardianship;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications when a guardianship is created by processing the associated event.
/// </summary>
/// <remarks>This event handler is typically used within a MediatR pipeline to respond to guardianship creation
/// events. It implements the INotificationHandler interface for GuardianshipCreatedEvent, enabling integration with
/// event-driven architectures. Thread safety and execution context are managed by the underlying MediatR
/// framework.</remarks>
public sealed class GuardianshipCreatedEventHandler : INotificationHandler<GuardianshipCreatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(GuardianshipCreatedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
