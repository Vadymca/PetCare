namespace PetCare.Domain.ValueObjects;

/// <summary>
/// Represents configuration settings for profile photo validation.
/// </summary>
/// <param name="maxSizeBytes">Maximum allowed file size in bytes.</param>
/// <param name="allowedExtensions">Array of allowed file extensions.</param>
public record ProfilePhotoConfig(long maxSizeBytes, string[] allowedExtensions)
{
    /// <summary>
    /// Gets the default profile photo configuration with 5 MB max size and allowed extensions ".jpg" and ".png".
    /// </summary>
    public static ProfilePhotoConfig Default => new(5 * 1024 * 1024, new[] { ".jpg", ".png" });
}
