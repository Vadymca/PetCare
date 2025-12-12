namespace PetCare.Api.Endpoints.AnimalAidRequests;

using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Features.AnimalAidRequests.GetBySlug;

/// <summary>
/// Endpoint for retrieving details of a specific animal aid request by slug.
/// </summary>
public static class GetAnimalAidRequestBySlugEndpoint
{
    /// <summary>
    /// Maps GET /api/animal-aid-requests/{slug}.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapGetAnimalAidRequestBySlugEndpoint(this WebApplication app)
    {
        app.MapGet("/api/animal-aid-requests/{slug}", async (
            string slug,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAnimalAidRequestBySlugEndpoint");

            var result = await mediator.Send(new GetAnimalAidRequestBySlugCommand(slug));

            logger.LogInformation("Retrieved details for animal aid request with slug {Slug}", slug);

            return Results.Ok(result);
        })
        .WithName("GetAnimalAidRequestBySlug")
        .WithTags("AnimalAidRequests")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<AnimalAidRequestDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
