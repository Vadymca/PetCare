namespace PetCare.Application.Interfaces;

using System.Threading.Tasks;

/// <summary>
/// Represents a contract for cloud storage operations.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Uploads a file to the storage bucket.
    /// </summary>
    /// <param name="objectName">The file name to store.</param>
    /// <param name="data">The file content stream.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <returns>The public URL of the uploaded file.</returns>
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
