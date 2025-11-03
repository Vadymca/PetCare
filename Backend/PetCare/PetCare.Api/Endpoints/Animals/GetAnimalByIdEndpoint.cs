namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.GetAnimalById;

/// <summary>
/// Endpoint for retrieving details of a specific animal by Id.
/// </summary>
public static class GetAnimalByIdEndpoint
{
    /// <summary>
    /// Maps the GET /api/animals/{id} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetAnimalByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/animals/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAnimalByIdEndpoint");

            var result = await mediator.Send(new GetAnimalByIdCommand(id));

            logger.LogInformation("Retrieved details for animal {AnimalId}", id);

            return Results.Ok(result);
        })
        .WithName("GetAnimalById")
        .WithTags("Animals")
        .Produces<AnimalDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireRateLimiting("GlobalPolicy");
    }
}
