namespace PetCare.Application.EventHandlers.Guardianship;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications when a guardianship is activated by processing the associated event.
/// </summary>
/// <remarks>This handler is typically used within a MediatR pipeline to respond to guardianship activation
/// events. It implements the INotificationHandler interface for the GuardianshipActivatedEvent type, enabling
/// asynchronous event handling. Thread safety and execution context are managed by the MediatR framework.</remarks>
public sealed class GuardianshipActivatedEventHandler : INotificationHandler<GuardianshipActivatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(GuardianshipActivatedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}
