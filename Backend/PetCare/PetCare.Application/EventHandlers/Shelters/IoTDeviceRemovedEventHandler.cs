namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles IoTDeviceRemovedEvent.
/// </summary>
public sealed class IoTDeviceRemovedEventHandler : INotificationHandler<IoTDeviceRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(IoTDeviceRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
