namespace PetCare.Application.Features.Media.UploadMedia;

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles uploading media files and returning their URL.
/// Media type is automatically determined by file extension.
/// Validates file size and extension before uploading.
/// </summary>
public class UploadMediaHandler : IRequestHandler<UploadMediaCommand, string>
{
    /// <summary>
    /// Maximum allowed photo size in bytes (5 MB).
    /// </summary>
    private const long MaxPhotoSize = 5 * 1024 * 1024;

    /// <summary>
    /// Maximum allowed video size in bytes (50 MB).
    /// </summary>
    private const long MaxVideoSize = 50 * 1024 * 1024;

    /// <summary>
    /// Supported photo file extensions.
    /// </summary>
    private static readonly string[] PhotoExtensions = new[]
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff",
    };

    /// <summary>
    /// Supported video file extensions.
    /// </summary>
    private static readonly string[] VideoExtensions = new[]
    {
        ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm", ".mpeg",
    };

    private readonly IStorageService storageService;
    private readonly ILogger<UploadMediaHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadMediaHandler"/> class.
    /// </summary>
    /// <param name="storageService">The MinIO storage service used for saving files.</param>
    /// <param name="logger">The logger used to record informational messages.</param>
    public UploadMediaHandler(IStorageService storageService, ILogger<UploadMediaHandler> logger)
    {
        this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService), "Сервіс збереження файлів не може бути null.");
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger), "Логер не може бути null.");
    }

    /// <inheritdoc/>
    public async Task<string> Handle(UploadMediaCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
        {
            throw new ArgumentException("Файл не може бути порожнім.");
        }

        var extension = Path.GetExtension(request.File.FileName)?.ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new ArgumentException("Файл має містити розширення.");
        }

        string mediaType;
        long maxSizeBytes;

        if (PhotoExtensions.Contains(extension))
        {
            mediaType = "photo";
            maxSizeBytes = MaxPhotoSize;
        }
        else if (VideoExtensions.Contains(extension))
        {
            mediaType = "video";
            maxSizeBytes = MaxVideoSize;
        }
        else
        {
            throw new ArgumentException(
                $"Недопустимий формат файлу. Дозволені формати фото: {string.Join(", ", PhotoExtensions)}, відео: {string.Join(", ", VideoExtensions)}");
        }

        if (request.File.Length > maxSizeBytes)
        {
            throw new ArgumentException($"Файл перевищує максимальний розмір {maxSizeBytes / (1024 * 1024)} MB для {mediaType}.");
        }

        await using var stream = request.File.OpenReadStream();

        // Викликаємо MinIO-сервіс
        var url = await this.storageService.UploadFileAsync(stream, request.File.FileName, request.File.ContentType);

        this.logger.LogInformation(
            "Uploaded {MediaType} '{File}' ({Size} bytes) -> {Url}",
            mediaType,
            request.File.FileName,
            request.File.Length,
            url);

        return url;
    }
}
