namespace PetCare.Application.Interfaces;

using System.Threading.Tasks;

/// <summary>
/// Represents a contract for cloud storage operations.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Asynchronously uploads a file to the storage service with the specified object name and content type.
    /// </summary>
    /// <param name="objectName">The name to assign to the uploaded object in the storage service. Cannot be null or empty.</param>
    /// <param name="data">A stream containing the file data to upload. The stream must be readable and positioned at the start of the data
    /// to upload. Cannot be null.</param>
    /// <param name="contentType">The MIME type of the file being uploaded. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the URI of the uploaded object as a
    /// string.</returns>
    Task<string> UploadFileAsync(string objectName, Stream data, string contentType);

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
