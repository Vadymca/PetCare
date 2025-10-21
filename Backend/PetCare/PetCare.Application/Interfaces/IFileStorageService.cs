namespace PetCare.Application.Interfaces;

using System.Threading.Tasks;

/// <summary>
/// Represents a service for storing and managing files.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Asynchronously uploads a file to the storage provider and returns a unique identifier or URL for the uploaded
    /// file.
    /// </summary>
    /// <param name="fileStream">The stream containing the file data to upload. Must be readable and positioned at the start of the file content.</param>
    /// <param name="originalFileName">The original name of the file, including extension. Used to preserve file metadata and may affect how the file
    /// is stored or accessed.</param>
    /// <param name="contentType">The MIME type of the file, such as "image/png" or "application/pdf". Determines how the file is handled and
    /// served by the storage provider.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a string with the unique identifier
    /// or URL of the uploaded file.</returns>
    Task<string> UploadAsync(Stream fileStream, string originalFileName, string contentType);

    /// <summary>
    /// Deletes a file from the storage by its URL.
    /// </summary>
    /// <param name="fileUrl">The URL of the file to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(string fileUrl);
}
