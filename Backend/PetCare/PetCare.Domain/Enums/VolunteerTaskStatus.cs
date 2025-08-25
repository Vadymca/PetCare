namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the current status of a volunteer task.
/// </summary>
public enum VolunteerTaskStatus
{
    /// <summary>
    /// The volunteer task is open and available for volunteers to take.
    /// </summary>
    Open,

    /// <summary>
    /// The volunteer task is currently in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The volunteer task has been completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The volunteer task has been cancelled and will not be completed.
    /// </summary>
    Cancelled,
}
