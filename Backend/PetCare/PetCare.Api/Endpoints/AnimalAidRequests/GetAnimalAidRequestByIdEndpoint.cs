namespace PetCare.Api.Endpoints.AnimalAidRequests;

using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Features.AnimalAidRequests.GetById;

/// <summary>
/// Endpoint for retrieving details of a specific animal aid request by ID.
/// </summary>
public static class GetAnimalAidRequestByIdEndpoint
{
    /// <summary>
    /// Maps GET /api/animal-aid-requests/{id}.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapGetAnimalAidRequestByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/animal-aid-requests/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAnimalAidRequestByIdEndpoint");

            var result = await mediator.Send(new GetAnimalAidRequestByIdCommand(id));

            logger.LogInformation("Retrieved details for animal aid request {RequestId}", id);

            return Results.Ok(result);
        })
        .WithName("GetAnimalAidRequestById")
        .WithTags("AnimalAidRequests")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<AnimalAidRequestDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
