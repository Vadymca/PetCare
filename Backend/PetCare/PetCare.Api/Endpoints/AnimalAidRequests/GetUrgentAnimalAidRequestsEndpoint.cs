namespace PetCare.Api.Endpoints.AnimalAidRequests;

using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Features.AnimalAidRequests.GetUrgent;

/// <summary>
/// Endpoint for retrieving all urgent animal aid requests.
/// </summary>
public static class GetUrgentAnimalAidRequestsEndpoint
{
    /// <summary>
    /// Maps GET /api/animal-aid-requests/urgent.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapGetUrgentAnimalAidRequestsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/animal-aid-requests/urgent", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetUrgentAnimalAidRequestsEndpoint");

            var result = await mediator.Send(new GetUrgentAnimalAidRequestsCommand());

            logger.LogInformation("Retrieved {Count} urgent animal aid requests", result.Count);

            return Results.Ok(result);
        })
        .WithName("GetUrgentAnimalAidRequests")
        .WithTags("AnimalAidRequests")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<List<UrgentAnimalAidRequestDto>>(StatusCodes.Status200OK);
    }
}
