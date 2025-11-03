namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.RemoveAnimalPhoto;

/// <summary>
/// Endpoint for removing a photo from a specific animal.
/// </summary>
public static class RemoveAnimalPhotoEndpoint
{
    /// <summary>
    /// Maps the DELETE /api/animals/{id}/photos/{photoUrl} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapRemoveAnimalPhotoEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/animals/{id:guid}/photos", async (
            Guid id,
            [FromQuery] string photoUrl,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RemoveAnimalPhotoEndpoint");

            var updatedAnimal = await mediator.Send(new RemoveAnimalPhotoCommand(id, photoUrl));

            logger.LogInformation("Removed photo from animal {AnimalId}", id);

            return Results.Ok(updatedAnimal);
        })
        .RequireAuthorization("CanManageAnimals") // ShelterManager or Admin
        .WithName("RemoveAnimalPhoto")
        .WithTags("Animals")
        .Produces<AnimalDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireRateLimiting("GlobalPolicy");
    }
}
