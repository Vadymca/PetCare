namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the current status of an animal in the system.
/// </summary>
public enum AnimalStatus
{
    /// <summary>
    /// The animal is available for adoption.
    /// </summary>
    Available,

    /// <summary>
    /// The animal has been adopted.
    /// </summary>
    Adopted,

    /// <summary>
    /// The animal is reserved for adoption.
    /// </summary>
    Reserved,

    /// <summary>
    /// The animal is currently undergoing treatment.
    /// </summary>
    InTreatment,

    /// <summary>
    /// The animal has passed away.
    /// </summary>
    Dead,

    /// <summary>
    /// The animal has been euthanized.
    /// </summary>
    Euthanized,
}
