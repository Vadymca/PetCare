namespace PetCare.Domain.Abstractions;

/// <summary>
/// Represents a service for storing and managing files.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file to the storage.
    /// </summary>
    /// <param name="fileStream">The file content stream.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="maxSizeBytes">The maximum allowed file size in bytes.</param>
    /// <param name="allowedExtensions">An array of allowed file extensions.</param>
    /// <returns>The URL of the uploaded file.</returns>
    Task<string> UploadAsync(Stream fileStream, string fileName, long maxSizeBytes, string[] allowedExtensions);

    /// <summary>
    /// Deletes a file from the storage by its URL.
    /// </summary>
    /// <param name="fileUrl">The URL of the file to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(string fileUrl);
}
