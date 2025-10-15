namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Domain service for handling IoT devices across shelters,
/// including cross-aggregate validation and notifications.
/// </summary>
public class IoTDeviceService : IIoTDeviceService
{
    private readonly INotificationService notificationService;
    private readonly IShelterRepository shelterRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="IoTDeviceService"/> class.
    /// </summary>
    /// <param name="notificationService">The notification service used to send alerts.</param>
    /// <param name="shelterRepository">The shelter repository for cross-aggregate checks.</param>
    /// <exception cref="ArgumentNullException">Thrown when any dependency is null.</exception>
    public IoTDeviceService(
        INotificationService notificationService,
        IShelterRepository shelterRepository)
    {
        this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        this.shelterRepository = shelterRepository ?? throw new ArgumentNullException(nameof(shelterRepository));
    }

    /// <summary>
    /// Adds an IoT device to a shelter, performing cross-aggregate validation.
    /// </summary>
    /// <param name="shelter">The shelter to which the device will be added.</param>
    /// <param name="device">The IoT device to add.</param>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shelter"/> or <paramref name="device"/> is null.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the shelter has reached maximum device capacity
    /// or if the device is already assigned to another shelter.
    /// </exception>
    public async Task AddDeviceToShelterAsync(Shelter shelter, IoTDevice device, Guid userId)
    {
        if (shelter is null)
        {
            throw new ArgumentNullException(nameof(shelter));
        }

        if (device is null)
        {
            throw new ArgumentNullException(nameof(device));
        }

        if (shelter.IoTDevices.Count >= 10)
        {
            throw new InvalidOperationException("Притулок досяг максимальної кількості IoT-пристроїв.");
        }

        var existingShelterWithDevice = await this.shelterRepository.GetShelterByDeviceIdAsync(device.Id);
        if (existingShelterWithDevice != null)
        {
            throw new InvalidOperationException(
                $"IoT device {device.Id} вже прив'язаний до притулку {existingShelterWithDevice.Id}");
        }

        // Крос-агрегатна логіка: перевірка, що пристрій ще не прив'язаний до іншого Shelter
        shelter.AddIoTDevice(device, userId);

        // Крос-агрегатна дія: сповіщення адміністрації (можна реалізувати через INotificationService)
    }

    /// <summary>
    /// Removes an IoT device from a shelter.
    /// </summary>
    /// <param name="shelter">The shelter from which the device will be removed.</param>
    /// <param name="deviceId">The ID of the IoT device to remove.</param>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shelter"/> is null.</exception>
    public async Task RemoveDeviceFromShelterAsync(Shelter shelter, Guid deviceId, Guid userId)
    {
        if (shelter is null)
        {
            throw new ArgumentNullException(nameof(shelter));
        }

        // Агрегат самостійно видаляє пристрій
        await Task.Run(() => shelter.RemoveIoTDevice(deviceId, userId));
    }
}
