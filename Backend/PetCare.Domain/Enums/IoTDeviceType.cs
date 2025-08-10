namespace PetCare.Domain.Enums;

/// <summary>
/// Represents different types of IoT devices used in the system.
/// </summary>
public enum IoTDeviceType
{
    /// <summary>
    /// A device that dispenses food.
    /// </summary>
    Feeder,

    /// <summary>
    /// A device that monitors temperature.
    /// </summary>
    Temperature,

    /// <summary>
    /// A device that captures video or images.
    /// </summary>
    Camera,
}
