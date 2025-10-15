namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides operations for managing IoT devices across shelters and other related aggregates.
/// </summary>
public interface IIoTDeviceService
{
    /// <summary>
    /// Adds an IoT device to the shelter, performing all necessary cross-aggregate validations.
    /// </summary>
    /// <param name="shelter">The shelter to which the device will be added.</param>
    /// <param name="device">The IoT device to add.</param>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddDeviceToShelterAsync(Shelter shelter, IoTDevice device, Guid userId);

    /// <summary>
    /// Removes an IoT device from the shelter.
    /// </summary>
    /// <param name="shelter">The shelter from which the device will be removed.</param>
    /// <param name="deviceId">The ID of the device to remove.</param>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveDeviceFromShelterAsync(Shelter shelter, Guid deviceId, Guid userId);
}
