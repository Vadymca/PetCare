namespace PetCare.Infrastructure.Services;

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PetCare.Application.Interfaces;

/// <summary>
/// Local file storage implementation of <see cref="IFileStorageService"/>.
/// Stores files in wwwroot/uploads and returns public URLs.
/// </summary>
public sealed class FileStorageService : IFileStorageService
{
    private readonly string uploadsFolder;
    private readonly string publicBaseUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileStorageService"/> class.
    /// </summary>
    /// <param name="environment">The hosting environment to resolve wwwroot path.</param>
    /// <param name="configuration">The application configuration to read the public base URL.</param>
    public FileStorageService(IWebHostEnvironment environment, IConfiguration configuration)
    {
        if (environment == null)
        {
            throw new ArgumentNullException(nameof(environment), "Середовище хостингу не може бути null.");
        }

        this.uploadsFolder = Path.Combine(environment.WebRootPath, "uploads");
        if (!Directory.Exists(this.uploadsFolder))
        {
            Directory.CreateDirectory(this.uploadsFolder);
        }

        // читаємо публічний базовий URL із конфігурації
        this.publicBaseUrl = configuration["PUBLIC_BASE_URL"] ?? "http://localhost:5100";
    }

    /// <inheritdoc/>
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

        // формуємо публічне посилання
        return $"{this.publicBaseUrl}/uploads/{uniqueFileName}";
    }

    /// <inheritdoc/>
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