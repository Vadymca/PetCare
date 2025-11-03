namespace PetCare.Api.Endpoints.Shelters;

using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.GetShelterBySlug;

/// <summary>
/// Endpoint for retrieving details of a specific shelter by slug.
/// </summary>
public static class GetShelterBySlugEndpoint
{
    /// <summary>
    /// Maps the GET /api/shelters/{slug} endpoint to retrieve shelter details by slug.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetShelterBySlugEndpoint(this WebApplication app)
    {
        app.MapGet("/api/shelters/{slug}", async (
            string slug,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetShelterBySlugEndpoint");

            var result = await mediator.Send(new GetShelterBySlugCommand(slug));

            logger.LogInformation("Retrieved details for shelter {Slug}", slug);

            return Results.Ok(result);
        })
        .WithName("GetShelterBySlug")
        .WithTags("Shelters")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<ShelterDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
