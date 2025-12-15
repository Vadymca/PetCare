namespace PetCare.Api.Endpoints.AdoptionApplications;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Features.AdoptionApplications.GetMy;

/// <summary>
/// Endpoint for retrieving the current user's adoption applications via JWT.
/// </summary>
public static class GetMyAdoptionApplicationsEndpoint
{
    /// <summary>
    /// Maps GET /api/adoption-applications/my.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetMyAdoptionApplicationsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/adoption-applications/my", async (
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetMyAdoptionApplicationsEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to /api/adoption-applications/my");
                return Results.Unauthorized();
            }

            var applications = await mediator.Send(new GetMyAdoptionApplicationsCommand(userId));

            logger.LogInformation("Retrieved {Count} adoption applications for user {UserId}", applications.Count, userId);

            return Results.Ok(applications);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetMyAdoptionApplications")
        .WithTags("AdoptionApplications")
        .Produces<IReadOnlyList<AdoptionApplicationListDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
