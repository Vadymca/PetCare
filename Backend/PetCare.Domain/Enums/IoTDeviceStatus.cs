// <copyright file="IoTDeviceStatus.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the operational status of an IoT device.
/// </summary>
public enum IoTDeviceStatus
{
    /// <summary>
    /// The device is active and functioning properly.
    /// </summary>
    Active,

    /// <summary>
    /// The device is inactive or turned off.
    /// </summary>
    Inactive,

    /// <summary>
    /// The device has encountered an error or malfunction.
    /// </summary>
    Error,
}
