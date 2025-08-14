namespace PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles IoTDeviceRemovedEvent.
/// </summary>
public sealed class IoTDeviceRemovedEventHandler : IDomainEventHandler<IoTDeviceRemovedEvent>
{
    /// <inheritdoc/>
    public async Task HandleAsync(IoTDeviceRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Логіка
        await Task.CompletedTask;
    }
}
