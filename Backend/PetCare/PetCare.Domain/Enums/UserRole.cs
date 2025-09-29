namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the roles assigned to users within the system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// A regular user with standard permissions.
    /// </summary>
    User,

    /// <summary>
    /// An administrator with full system access.
    /// </summary>
    Admin,

    /// <summary>
    /// A moderator responsible for managing content and users.
    /// </summary>
    Moderator,

    /// <summary>
    /// A manager responsible for overseeing shelters.
    /// </summary>
    ShelterManager,

    /// <summary>
    /// A veterinarian who can manage animal health records.
    /// </summary>
    Veterinarian,

    /// <summary>
    /// A volunteer who can perform tasks and aid requests.
    /// </summary>
    Volunteer,
}
