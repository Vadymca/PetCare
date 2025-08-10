namespace PetCare.Domain.ValueObjects;

/// <summary>
/// Represents configuration settings for media validation (photos, videos).
/// </summary>
/// <param name="maxSizeBytes">Maximum allowed file size in bytes.</param>
/// <param name="allowedExtensions">Array of allowed file extensions.</param>
public record MediaConfig(long maxSizeBytes, string[] allowedExtensions)
{
    /// <summary>
    /// Gets the default photo media configuration with 5 MB max size and allowed extensions ".jpg" and ".png".
    /// </summary>
    public static MediaConfig PhotoDefault => new(5 * 1024 * 1024, new[] { ".jpg", ".png" });

    /// <summary>
    /// Gets the default video media configuration with 50 MB max size and allowed extensions ".mp4", ".avi".
    /// </summary>
    public static MediaConfig VideoDefault => new(50 * 1024 * 1024, new[] { ".mp4", ".avi" });

    /// <summary>
    /// Validates the file size and extension.
    /// </summary>
    /// <param name="fileName">File name to check extension.</param>
    /// <param name="fileSizeBytes">File size in bytes.</param>
    /// <exception cref="ArgumentException">Thrown when file size or extension is invalid.</exception>
    public void Validate(string fileName, long fileSizeBytes)
    {
        if (fileSizeBytes > this.maxSizeBytes)
        {
            throw new ArgumentException($"Файл перевищує максимальний розмір {this.maxSizeBytes} байт.");
        }

        var extension = System.IO.Path.GetExtension(fileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !this.allowedExtensions.Contains(extension))
        {
            throw new ArgumentException($"Недопустиме розширення файлу. Дозволені: {string.Join(", ", this.allowedExtensions)}");
        }
    }
}
