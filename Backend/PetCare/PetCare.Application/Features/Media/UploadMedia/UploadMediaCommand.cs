namespace PetCare.Application.Features.Media.UploadMedia;

using MediatR;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Represents a request to upload a media file and returns the URL or identifier of the uploaded media.
/// </summary>
/// <param name="File">The media file to upload. Must not be null and should contain the content to be stored.</param>
public record UploadMediaCommand(
    IFormFile File)
    : IRequest<string>;
