namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Features.AdoptionApplications.GetAll;

/// <summary>
/// Endpoint for retrieving the list of all adoption applications.
/// </summary>
public static class GetAllAdoptionApplicationsEndpoint
{
    /// <summary>
    /// Maps GET /api/adoption-applications.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapGetAllAdoptionApplicationsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/adoption-applications", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAllAdoptionApplicationsEndpoint");

            var result = await mediator.Send(new GetAdoptionApplicationsCommand());

            logger.LogInformation("Retrieved list of adoption applications. Count: {Count}", result.Count);

            return Results.Ok(result);
        })
        .WithName("GetAllAdoptionApplications")
        .WithTags("AdoptionApplications")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<IReadOnlyList<AdoptionApplicationListDto>>(StatusCodes.Status200OK);
    }
}
