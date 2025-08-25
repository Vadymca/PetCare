namespace PetCare.Application.EventHandlers.Shelters;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles IoTDeviceAddedEvent.
/// </summary>

public sealed class IoTDeviceAddedEventHandler : INotificationHandler<IoTDeviceAddedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(IoTDeviceAddedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
