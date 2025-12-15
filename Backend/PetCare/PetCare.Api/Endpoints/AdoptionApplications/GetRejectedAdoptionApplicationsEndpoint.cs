namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Features.AdoptionApplications.GetRejected;

/// <summary>
/// Endpoint for retrieving all rejected adoption applications.
/// </summary>
public static class GetRejectedAdoptionApplicationsEndpoint
{
    /// <summary>
    /// Maps GET /api/adoption-applications/rejected.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetRejectedAdoptionApplicationsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/adoption-applications/rejected", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetRejectedAdoptionApplicationsEndpoint");

            var applications = await mediator.Send(new GetRejectedAdoptionApplicationsCommand());

            logger.LogInformation("Retrieved {Count} rejected adoption applications.", applications.Count);

            return Results.Ok(applications);
        })
        .WithName("GetRejectedAdoptionApplications")
        .WithTags("AdoptionApplications")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<IReadOnlyList<AdoptionApplicationListDto>>(StatusCodes.Status200OK);
    }
}
