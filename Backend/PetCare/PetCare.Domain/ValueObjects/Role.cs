namespace PetCare.Domain.ValueObjects;

using PetCare.Domain.Enums;

/// <summary>
/// Represents a strongly-typed user role in the domain.
/// </summary>
public record Role(UserRole Value)
{
    /// <summary>
    /// Gets the default role for regular users.
    /// </summary>
    public static Role User => new(UserRole.User);

    /// <summary>
    /// Gets the administrator role with the highest privileges.
    /// </summary>
    public static Role Admin => new(UserRole.Admin);

    /// <summary>
    /// Gets the moderator role with content and community management privileges.
    /// </summary>
    public static Role Moderator => new(UserRole.Moderator);

    /// <inheritdoc/>
    public override string ToString() => this.Value.ToString();
}
