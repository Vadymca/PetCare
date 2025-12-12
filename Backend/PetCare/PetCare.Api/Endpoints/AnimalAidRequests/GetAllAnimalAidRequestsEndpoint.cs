namespace PetCare.Api.Endpoints.AnimalAidRequests;

using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Features.AnimalAidRequests.GetAllAnimalAidRequests;

/// <summary>
/// Endpoint for retrieving the list of all animal aid requests.
/// </summary>
public static class GetAllAnimalAidRequestsEndpoint
{
    /// <summary>
    /// Maps GET /api/animal-aid-requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapGetAllAnimalAidRequestsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/animal-aid-requests", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAllAnimalAidRequestsEndpoint");

            var result = await mediator.Send(new GetAllAnimalAidRequestsCommand());

            logger.LogInformation("Retrieved list of animal aid requests. Count: {Count}", result.Count);

            return Results.Ok(result);
        })
        .WithName("GetAllAnimalAidRequests")
        .WithTags("AnimalAidRequests")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<IReadOnlyList<AnimalAidRequestListDto>>(StatusCodes.Status200OK);
    }
}
