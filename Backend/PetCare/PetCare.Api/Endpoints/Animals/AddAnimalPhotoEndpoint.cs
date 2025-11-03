namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.AddAnimalPhoto;

/// <summary>
/// Endpoint for adding a photo to a specific animal.
/// </summary>
public static class AddAnimalPhotoEndpoint
{
    /// <summary>
    /// Maps the POST /api/animals/{id}/photos endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapAddAnimalPhotoEndpoint(this WebApplication app)
    {
        app.MapPost("/api/animals/{id:guid}/photos", async (
            Guid id,
            AddAnimalPhotobody body,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AddAnimalPhotoEndpoint");

            var result = await mediator.Send(new AddAnimalPhotoCommand(id, body.PhotoUrl));

            logger.LogInformation("Added photo to animal {AnimalId}", id);

            return Results.Ok(result);
        })
        .RequireAuthorization("CanManageAnimals") // ShelterManager or Admin
        .WithName("AddAnimalPhoto")
        .WithTags("Animals")
        .Produces<AnimalDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireRateLimiting("GlobalPolicy");
    }
}
