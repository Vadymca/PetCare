namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Features.AdoptionApplications.GetById;

/// <summary>
/// Endpoint for retrieving details of a specific adoption application by ID.
/// </summary>
public static class GetAdoptionApplicationByIdEndpoint
{
    /// <summary>
    /// Maps GET /api/adoption-applications/{id}.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapGetAdoptionApplicationByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/adoption-applications/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAdoptionApplicationByIdEndpoint");

            var result = await mediator.Send(new GetAdoptionApplicationByIdCommand(id));

            logger.LogInformation("Retrieved adoption application details for {ApplicationId}", id);

            return Results.Ok(result);
        })
        .WithName("GetAdoptionApplicationById")
        .WithTags("AdoptionApplications")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<AdoptionApplicationDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
