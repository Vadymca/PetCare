namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.GetAnimalBySlug;

/// <summary>
/// Endpoint for retrieving details of a specific animal by Slug.
/// </summary>
public static class GetAnimalBySlugEndpoint
{
    /// <summary>
    /// Maps the GET /api/animals/{slug} endpoint to retrieve animal details by slug.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetAnimalBySlugEndpoint(this WebApplication app)
    {
        app.MapGet("/api/animals/{slug}", async (
            string slug,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAnimalBySlugEndpoint");

            var result = await mediator.Send(new GetAnimalBySlugCommand(slug));

            logger.LogInformation("Retrieved details for animal {Slug}", slug);

            return Results.Ok(result);
        })
        .WithName("GetAnimalBySlug")
        .WithTags("Animals")
        .Produces<AnimalDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireRateLimiting("GlobalPolicy");
    }
}
