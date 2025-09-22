namespace PetCare.Api.Endpoints.Media;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Media.UploadMedia;

/// <summary>
/// Contains endpoint mapping for media upload (photos and videos).
/// Validates file size and type before sending to the handler.
/// </summary>
public static class UploadMediaEndpoint
{
    /// <summary>
    /// Maps the POST /api/media/upload endpoint to handle media upload requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapUploadMediaEndpoint(this WebApplication app)
    {
        app.MapPost("/api/media/upload", async (IMediator mediator, HttpRequest request, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UploadMediaEndpoint");

            if (!request.HasFormContentType || !request.Form.Files.Any())
            {
                logger.LogWarning("No file provided in the request.");
                return Results.BadRequest("Файл не надано.");
            }

            var file = request.Form.Files[0];

            // Перевірка розміру файлу до обробки
            const long maxSizeBytes = 50 * 1024 * 1024; // 50 MB
            if (file.Length == 0)
            {
                return Results.BadRequest("Файл не може бути порожнім.");
            }

            if (file.Length > maxSizeBytes)
            {
                return Results.StatusCode(StatusCodes.Status413PayloadTooLarge);
            }

            var command = new UploadMediaCommand(file);

            logger.LogInformation("Uploading file: {FileName}, size: {Size} bytes", file.FileName, file.Length);

            var url = await mediator.Send(command);

            logger.LogInformation("Media uploaded successfully: {Url}", url);

            return Results.Ok(new { Url = url });
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("UploadMedia")
        .WithTags("Media")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status413PayloadTooLarge)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<IFormFile>("multipart/form-data")
        .WithMetadata(new RequestSizeLimitAttribute(50 * 1024 * 1024));
    }
}
