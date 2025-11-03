namespace PetCare.Api.Endpoints.Shelters;

using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.GetShelterById;

/// <summary>
/// Endpoint for retrieving shelter details by ID.
/// </summary>
public static class GetShelterByIdEndpoint
{
    /// <summary>
    /// Maps GET /api/shelters/{id}.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetShelterByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/shelters/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetShelterByIdEndpoint");

            var result = await mediator.Send(new GetShelterByIdCommand(id));

            logger.LogInformation("Retrieved details for shelter {ShelterId}", id);

            return Results.Ok(result);
        })
        .WithName("GetShelterById")
        .WithTags("Shelters")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<ShelterDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
