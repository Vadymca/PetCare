namespace PetCare.Api.Endpoints.Payments;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.GetMyUpcomingPayments;

/// <summary>
/// Endpoint for fetching the current user's upcoming scheduled payments.
/// </summary>
public static class GetMyUpcomingPaymentsEndpoint
{
    /// <summary>
    /// Maps the GET /api/payments/me/upcoming endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetMyUpcomingPaymentsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/payments/me/upcoming", async (
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetMyUpcomingPaymentsEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to /api/payments/me/upcoming");
                return Results.Unauthorized();
            }

            var upcoming = await mediator.Send(new GetMyUpcomingPaymentsCommand(userId));
            logger.LogInformation("Fetched {Count} upcoming payments for user {UserId}", upcoming.Count, userId);

            return Results.Ok(upcoming);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetMyUpcomingPayments")
        .WithTags("Payments")
        .Produces<IReadOnlyList<MyUpcomingPaymentDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
