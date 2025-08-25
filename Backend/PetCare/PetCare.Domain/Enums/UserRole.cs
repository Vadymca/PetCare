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
}
