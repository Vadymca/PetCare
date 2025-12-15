namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Features.AdoptionApplications.GetApproved;

/// <summary>
/// Endpoint for retrieving the list of approved adoption applications.
/// </summary>
public static class GetApprovedAdoptionApplicationsEndpoint
{
    /// <summary>
    /// Maps GET /api/adoption-applications/approved.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapGetApprovedAdoptionApplicationsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/adoption-applications/approved", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetApprovedAdoptionApplicationsEndpoint");

            var result = await mediator.Send(new GetApprovedAdoptionApplicationsCommand());

            logger.LogInformation("Retrieved list of approved adoption applications. Count: {Count}", result.Count);

            return Results.Ok(result);
        })
        .WithName("GetApprovedAdoptionApplications")
        .WithTags("AdoptionApplications")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<IReadOnlyList<AdoptionApplicationListDto>>(StatusCodes.Status200OK);
    }
}
