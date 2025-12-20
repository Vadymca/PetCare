namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Features.AdoptionApplications.GetPending;

/// <summary>
/// Endpoint for retrieving all pending adoption applications.
/// </summary>
public static class GetPendingAdoptionApplicationsEndpoint
{
    /// <summary>
    /// Maps GET /api/adoption-applications/pending.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetPendingAdoptionApplicationsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/adoption-applications/pending", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetPendingAdoptionApplicationsEndpoint");

            var applications = await mediator.Send(new GetPendingAdoptionApplicationsCommand());

            logger.LogInformation("Retrieved {Count} pending adoption applications.", applications.Count);

            return Results.Ok(applications);
        })
        .WithName("GetPendingAdoptionApplications")
        .WithTags("AdoptionApplications")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<IReadOnlyList<AdoptionApplicationDetailsDto>>(StatusCodes.Status200OK);
    }
}
