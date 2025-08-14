namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles IoTDeviceAddedEvent.
/// </summary>

public sealed class IoTDeviceAddedEventHandler : IDomainEventHandler<IoTDeviceAddedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(IoTDeviceAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
