namespace PetCare.Application.Interfaces;

using System.Threading.Tasks;

/// <summary>
/// Represents a contract for cloud storage operations.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Asynchronously uploads a file to the storage service using the provided data stream and metadata.
    /// </summary>
    /// <param name="data">The stream containing the file data to upload. The stream must be readable and positioned at the start of the
    /// file content.</param>
    /// <param name="originalFileName">The original name of the file, including the file extension. Used to preserve file metadata and determine
    /// storage naming.</param>
    /// <param name="contentType">The MIME type of the file (for example, "image/png" or "application/pdf"). Determines how the file will be
    /// handled and served.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a string with the unique identifier
    /// or URL of the uploaded file.</returns>
    Task<string> UploadFileAsync(Stream data, string originalFileName, string contentType);

    /// <summary>
    /// Downloads a file from the storage bucket.
    /// </summary>
    /// <param name="objectName">The name of the file to download.</param>
    /// <returns>The file content stream.</returns>
    Task<Stream> DownloadFileAsync(string objectName);

    /// <summary>
    /// Deletes a file from the storage bucket.
    /// </summary>
    /// <param name="objectName">The name of the file to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteFileAsync(string objectName);

    /// <summary>
    /// Generates a presigned URL for a file with limited lifetime.
    /// </summary>
    /// <param name="objectName">The file name.</param>
    /// <param name="expirySeconds">URL expiration time in seconds.</param>
    /// <returns>A presigned URL string.</returns>
    Task<string> GeneratePresignedUrlAsync(string objectName, int expirySeconds = 3600);
}
