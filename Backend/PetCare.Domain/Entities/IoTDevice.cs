// <copyright file="IoTDevice.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;

/// <summary>
/// Represents an IoT device in the system.
/// </summary>
public sealed class IoTDevice : BaseEntity
{
    private IoTDevice()
    {
    }

    private IoTDevice(
        Guid shelterId,
        IoTDeviceType type,
        string name,
        IoTDeviceStatus status,
        string serialNumber,
        Dictionary<string, object>? data,
        Dictionary<string, object>? alertThresholds)
    {
        if (shelterId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор притулку не може бути порожнім.", nameof(shelterId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Назва не може бути пустою.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(serialNumber))
        {
            throw new ArgumentException("Серійний номер не може бути порожнім.", nameof(serialNumber));
        }

        this.ShelterId = shelterId;
        this.Type = type;
        this.Name = name;
        this.Status = status;
        this.SerialNumber = serialNumber;
        this.Data = data;
        this.AlertThresholds = alertThresholds;
        this.LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the type of the IoT device.
    /// </summary>
    public IoTDeviceType Type { get; private set; }

    /// <summary>
    /// Gets the name of the IoT device.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// Gets the current status of the IoT device.
    /// </summary>
    public IoTDeviceStatus Status { get; private set; }

    /// <summary>
    /// Gets the data collected by the IoT device, if any. Can be null.
    /// </summary>
    public Dictionary<string, object>? Data { get; private set; }

    /// <summary>
    /// Gets the serial number of the IoT device.
    /// </summary>
    public string SerialNumber { get; private set; } = default!;

    /// <summary>
    /// Gets the alert thresholds for the IoT device, if any. Can be null.
    /// </summary>
    public Dictionary<string, object>? AlertThresholds { get; private set; }

    /// <summary>
    /// Gets the date and time when the IoT device was last updated.
    /// </summary>
    public DateTime LastUpdated { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter associated with the device.
    /// </summary>
    public Guid ShelterId { get; private set; }

    /// <summary>
    /// Gets navigation property to the shelter.
    /// </summary>
    public Shelter? Shelter { get; private set; }

    /// <summary>
    /// Creates a new <see cref="IoTDevice"/> instance with the specified parameters.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter associated with the device.</param>
    /// <param name="type">The type of the IoT device.</param>
    /// <param name="name">The name of the IoT device.</param>
    /// <param name="status">The current status of the IoT device.</param>
    /// <param name="serialNumber">The serial number of the IoT device.</param>
    /// <param name="data">The data collected by the IoT device, if any. Can be null.</param>
    /// <param name="alertThresholds">The alert thresholds for the IoT device, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="IoTDevice"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="shelterId"/> is an empty GUID, or <paramref name="name"/> or <paramref name="serialNumber"/> is null or whitespace.</exception>
    public static IoTDevice Create(
        Guid shelterId,
        IoTDeviceType type,
        string name,
        IoTDeviceStatus status,
        string serialNumber,
        Dictionary<string, object>? data = null,
        Dictionary<string, object>? alertThresholds = null)
    {
        return new IoTDevice(
            shelterId,
            type,
            name,
            status,
            serialNumber,
            data,
            alertThresholds);
    }

    /// <summary>
    /// Updates the data collected by the IoT device.
    /// </summary>
    /// <param name="newData">The new data for the IoT device. Can be null.</param>
    public void UpdateData(Dictionary<string, object> newData)
    {
        this.Data = newData;
        this.LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the status of the IoT device.
    /// </summary>
    /// <param name="status">The new status of the IoT device.</param>
    public void UpdateStatus(IoTDeviceStatus status)
    {
        this.Status = status;
        this.LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the alert thresholds for the IoT device.
    /// </summary>
    /// <param name="thresholds">The new alert thresholds for the IoT device. Can be null.</param>
    public void UpdateThresholds(Dictionary<string, object> thresholds)
    {
        this.AlertThresholds = thresholds;
        this.LastUpdated = DateTime.UtcNow;
    }
}
