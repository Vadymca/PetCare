namespace PetCare.Api.Endpoints.Payments;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.GetMyGuardianships;

/// <summary>
/// Endpoint for fetching the current user's guardianships.
/// </summary>
public static class GetMyGuardianshipsEndpoint
{
    /// <summary>
    /// Maps the GET /api/guardianships/me endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetMyGuardianshipsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/guardianships/me", async (
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetMyGuardianshipsEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to /api/guardianships/me");
                return Results.Unauthorized();
            }

            var guardianships = await mediator.Send(new GetMyGuardianshipsCommand(userId));

            logger.LogInformation("Fetched {Count} guardianships for current user {UserId}", guardianships.Count, userId);

            return Results.Ok(guardianships);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetMyGuardianships")
        .WithTags("Payments")
        .Produces<IReadOnlyList<MyGuardianshipDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
