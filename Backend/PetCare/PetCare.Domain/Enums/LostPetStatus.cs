namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the current status of a lost pet report.
/// </summary>
public enum LostPetStatus
{
    /// <summary>
    /// The pet is currently reported as lost.
    /// </summary>
    Lost,

    /// <summary>
    /// The pet has been found but not yet reunited with the owner.
    /// </summary>
    Found,

    /// <summary>
    /// The pet has been reunited with its owner.
    /// </summary>
    Reunited,
}
