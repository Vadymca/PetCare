namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.UpdateAnimal;

/// <summary>
/// Endpoint for updating an existing animal.
/// </summary>
public static class UpdateAnimalEndpoint
{
    /// <summary>
    /// Maps the PUT /api/animals/{id} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapUpdateAnimalEndpoint(this WebApplication app)
    {
        app.MapPut("/api/animals/{id:guid}", async (
            Guid id,
            UpdateAnimalBody body,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UpdateAnimalEndpoint");

            var command = new UpdateAnimalCommand(
                Id: id,
                Name: body.Name,
                Birthday: body.Birthday,
                Gender: body.Gender,
                Description: body.Description,
                Status: body.Status,
                AdoptionRequirements: body.AdoptionRequirements,
                MicrochipId: body.MicrochipId,
                Weight: body.Weight,
                Height: body.Height,
                Color: body.Color,
                IsSterilized: body.IsSterilized,
                HaveDocuments: body.HaveDocuments,
                HealthConditions: body.HealthConditions,
                SpecialNeeds: body.SpecialNeeds,
                Temperaments: body.Temperaments,
                Size: body.Size,
                CareCost: body.CareCost);

            var updatedAnimal = await mediator.Send(command);

            logger.LogInformation("Animal {AnimalId} updated", id);

            return Results.Ok(updatedAnimal);
        })
        .RequireAuthorization("CanManageAnimals") // ShelterManager or Admin
        .WithName("UpdateAnimal")
        .WithTags("Animals")
        .Produces<AnimalDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .RequireRateLimiting("GlobalPolicy");
    }
}
