namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the current status of a donation.
/// </summary>
public enum DonationStatus
{
    /// <summary>
    /// The donation is pending and not yet processed.
    /// </summary>
    Pending,

    /// <summary>
    /// The donation has been successfully completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The donation process has failed.
    /// </summary>
    Failed,
}
