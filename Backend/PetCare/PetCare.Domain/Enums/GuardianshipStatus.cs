namespace PetCare.Domain.Enums;

/// <summary>
/// Specifies the status of a guardianship process, indicating whether payment is required, the process is active, or
/// has been completed.
/// </summary>
/// <remarks>Use this enumeration to track and manage the current state of a guardianship workflow. The values
/// correspond to distinct stages: payment requirement, active processing, and completion. This status can be used to
/// control application logic, such as enabling actions only when the guardianship is active or completed.</remarks>
public enum GuardianshipStatus
{
    /// <summary>
    /// Indicates that payment is required to access the associated resource or perform the specified operation.
    /// </summary>
    RequiresPayment = 0,

    /// <summary>
    /// Indicates that the item is currently active.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Indicates that the operation has completed successfully.
    /// </summary>
    Completed = 2,
}
