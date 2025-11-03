namespace PetCare.Api.Endpoints.Shelters;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.RemoveShelterPhoto;

/// <summary>
/// Endpoint for removing a photo from a specific shelter.
/// </summary>
public static class RemoveShelterPhotoEndpoint
{
    /// <summary>
    /// Maps the DELETE /api/shelters/{id}/photos endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapRemoveShelterPhotoEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/shelters/{id:guid}/photos", async (
            Guid id,
            [FromQuery] string photoUrl,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RemoveShelterPhotoEndpoint");

            var updatedShelter = await mediator.Send(new RemoveShelterPhotoCommand(id, photoUrl));

            logger.LogInformation("Removed photo from shelter {ShelterId}", id);

            return Results.Ok(updatedShelter);
        })
        .RequireAuthorization("CanManageShelter") // ShelterManager or Admin
        .RequireRateLimiting("GlobalPolicy")
        .WithName("RemoveShelterPhoto")
        .WithTags("Shelters")
        .Produces<ShelterDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
