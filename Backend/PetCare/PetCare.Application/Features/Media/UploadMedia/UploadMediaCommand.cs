namespace PetCare.Application.Features.Media.UploadMedia;

using MediatR;
using Microsoft.AspNetCore.Http;

public record UploadMediaCommand(
    IFormFile File)
    : IRequest<string>;
