namespace PetCare.Application.EventHandlers.Shelters;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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
