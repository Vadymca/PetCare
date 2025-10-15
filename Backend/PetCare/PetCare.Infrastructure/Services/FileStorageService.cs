namespace PetCare.Infrastructure.Services;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PetCare.Application.Interfaces;

/// <summary>
/// Implementation of IFileStorageService that stores files in wwwroot/uploads.
/// Handles large files efficiently using streaming.
/// </summary>
public class FileStorageService : IFileStorageService
{
    private readonly string uploadsFolder;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileStorageService"/> class.
    /// Ensures that the uploads folder exists.
    /// </summary>
    /// <param name="webRootPath">The web root path (wwwroot) of the application.</param>
    public FileStorageService(string webRootPath)
    {
        this.uploadsFolder = Path.Combine(webRootPath, "uploads");
        if (!Directory.Exists(this.uploadsFolder))
        {
            Directory.CreateDirectory(this.uploadsFolder);
        }
    }

    /// <summary>
    /// Uploads a file to the storage after validating its size and extension.
    /// Uses streaming to prevent memory overload with large files.
    /// </summary>
    /// <param name="fileStream">The stream containing the file content.</param>
    /// <param name="fileName">The original file name (used for extension validation).</param>
    /// <param name="maxSizeBytes">Maximum allowed file size in bytes.</param>
    /// <param name="allowedExtensions">Array of allowed file extensions (e.g., ".jpg", ".png").</param>
    /// <returns>The URL of the uploaded file to be used on the frontend.</returns>
    /// <exception cref="ArgumentException">Thrown when file size or extension is invalid.</exception>
    public async Task<string> UploadAsync(Stream fileStream, string fileName, long maxSizeBytes, string[] allowedExtensions)
    {
        if (fileStream == null || fileStream.Length == 0)
        {
            throw new ArgumentException("Файл не може бути порожнім.");
        }

        if (fileStream.Length > maxSizeBytes)
        {
            throw new ArgumentException($"Файл перевищує максимальний розмір {maxSizeBytes} байт.");
        }

        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
        {
            throw new ArgumentException($"Недопустиме розширення файлу. Дозволені: {string.Join(", ", allowedExtensions)}");
        }

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(this.uploadsFolder, uniqueFileName);

        // Асинхронне збереження файлу через стрім
        await using var outputStream = new FileStream(
            filePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);

        await fileStream.CopyToAsync(outputStream);

        return $"http://localhost:5000/uploads/{uniqueFileName}";
    }

    /// <summary>
    /// Deletes a file from storage by its URL.
    /// </summary>
    /// <param name="fileUrl">The URL of the file to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
