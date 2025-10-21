namespace PetCare.Infrastructure.Services;

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PetCare.Application.Interfaces;

/// <summary>
/// Implementation of IFileStorageService that stores files in wwwroot/uploads.
/// Handles large files efficiently using streaming.
/// </summary>
public sealed class FileStorageService : IFileStorageService
{
    private readonly string uploadsFolder;
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileStorageService"/> class.
    /// Ensures that the uploads folder exists.
    /// </summary>
    /// <param name="environment">The hosting environment to resolve wwwroot path.</param>
    /// <param name="httpContextAccessor">Used to build absolute file URLs.</param>
    public FileStorageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        if (environment == null)
        {
            throw new ArgumentNullException(nameof(environment), "Середовище хостингу не може бути null.");
        }

        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor), "Доступ до HttpContext не може бути null.");

        this.uploadsFolder = Path.Combine(environment.WebRootPath, "uploads");

        if (!Directory.Exists(this.uploadsFolder))
        {
            Directory.CreateDirectory(this.uploadsFolder);
        }
    }

    /// <summary>
    /// Asynchronously uploads a file to the server and returns the public URL for accessing the uploaded file.
    /// </summary>
    /// <remarks>The returned URL is constructed based on the current HTTP request's scheme and host. If the
    /// request context is unavailable, a default base URL is used. The uploaded file is saved with a unique name to
    /// prevent collisions.</remarks>
    /// <param name="fileStream">The stream containing the file data to upload. Must not be null or empty.</param>
    /// <param name="originalFileName">The original name of the file, including its extension. Used to determine the file type and extension.</param>
    /// <param name="contentType">The MIME type of the file being uploaded. Used to identify the content type of the file.</param>
    /// <returns>A string containing the public URL where the uploaded file can be accessed.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="fileStream"/> is null or empty, or if <paramref name="originalFileName"/> does not
    /// contain a file extension.</exception>
    public async Task<string> UploadAsync(Stream fileStream, string originalFileName, string contentType)
    {
        if (fileStream == null || fileStream.Length == 0)
        {
            throw new ArgumentException("Файл не може бути порожнім.");
        }

        var extension = Path.GetExtension(originalFileName)?.ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new ArgumentException("Файл має містити розширення.");
        }

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(this.uploadsFolder, uniqueFileName);

        await using var outputStream = new FileStream(
            filePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);

        await fileStream.CopyToAsync(outputStream);

        var request = this.httpContextAccessor.HttpContext?.Request;
        var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "http://localhost:5000";

        return $"{baseUrl}/uploads/{uniqueFileName}";
    }

    /// <summary>
    /// Deletes the file at the specified URL from the uploads folder asynchronously, if it exists.
    /// </summary>
    /// <remarks>If the file specified by <paramref name="fileUrl"/> does not exist in the uploads folder, the
    /// method completes without error. No exception is thrown if the file is missing or if <paramref name="fileUrl"/>
    /// is invalid.</remarks>
    /// <param name="fileUrl">The URL of the file to delete. If null, empty, or whitespace, no action is taken.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task is completed when the operation finishes.</returns>
    public Task DeleteAsync(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return Task.CompletedTask;
        }

        var fileName = Path.GetFileName(fileUrl);
        var filePath = Path.Combine(this.uploadsFolder, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}
