namespace PetCare.Api.Endpoints.Animals;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.CreateAnimal;

/// <summary>
/// Endpoint for creating a new animal.
/// Accessible by ShelterManager or Admin.
/// </summary>
public static class CreateAnimalEndpoint
{
    /// <summary>
    /// Maps the POST /api/animals endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapCreateAnimalEndpoint(this WebApplication app)
    {
        app.MapPost("/api/animals", async (
            CreateAnimalBody body,
            IMediator mediator,
            ILoggerFactory loggerFactory,
            HttpContext httpContext) =>
        {
            var logger = loggerFactory.CreateLogger("CreateAnimalEndpoint");

            // Витягуємо UserId з токена
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Results.Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim);

            var command = new CreateAnimalCommand(
                userId,
                body.Name,
                body.BreedId,
                body.Birthday,
                body.Gender,
                body.Description,
                body.HealthConditions,
                body.SpecialNeeds,
                body.Temperaments,
                body.Size,
                body.Photos,
                body.Videos,
                body.ShelterId,
                body.Status,
                body.CareCost,
                body.AdoptionRequirements,
                body.MicrochipId,
                body.Weight,
                body.Height,
                body.Color,
                body.IsSterilized,
                body.IsUnderCare,
                body.HaveDocuments);

            var addedAnimal = await mediator.Send(command);

            logger.LogInformation("Animal {AnimalId} created by user {UserId}", addedAnimal.Id, userId);

            return Results.Created($"/api/animals/{addedAnimal.Id}", addedAnimal);
        })
        .RequireAuthorization("CanManageAnimals") // ShelterManager or Admin
        .WithName("CreateAnimal")
        .WithTags("Animals")
        .Produces<AnimalDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .RequireRateLimiting("GlobalPolicy");
    }
}
