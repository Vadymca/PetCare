namespace PetCare.Api.Endpoints.Shelters;

using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.AddShelterPhoto;

/// <summary>
/// Endpoint for adding a photo to a shelter.
/// </summary>
public static class AddShelterPhotoEndpoint
{
    /// <summary>
    /// Maps the POST /api/shelters/{id}/photos endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapAddShelterPhotoEndpoint(this WebApplication app)
    {
        app.MapPost("/api/shelters/{id:guid}/photos", async (
            Guid id,
            AddShelterPhotoBody body,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AddShelterPhotoEndpoint");

            var result = await mediator.Send(new AddShelterPhotoCommand(id, body.PhotoUrl));

            logger.LogInformation("Added photo to shelter {ShelterId}", id);

            return Results.Ok(result);
        })
        .RequireAuthorization("CanManageShelter")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("AddShelterPhoto")
        .WithTags("Shelters")
        .Produces<ShelterDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
