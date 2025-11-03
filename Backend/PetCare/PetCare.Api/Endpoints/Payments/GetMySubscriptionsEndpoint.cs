namespace PetCare.Api.Endpoints.Payments;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.GetMySubscriptions;

/// <summary>
/// Endpoint for fetching the current user's recurring payment subscriptions.
/// </summary>
public static class GetMySubscriptionsEndpoint
{
    /// <summary>
    /// Maps the GET /api/payments/me/subscriptions endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetMySubscriptionsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/payments/me/subscriptions", async (
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetMySubscriptionsEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to /api/payments/me/subscriptions");
                return Results.Unauthorized();
            }

            var subscriptions = await mediator.Send(new GetMySubscriptionsCommand(userId));
            logger.LogInformation("Fetched {Count} subscriptions for user {UserId}", subscriptions.Count, userId);

            return Results.Ok(subscriptions);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetMySubscriptions")
        .WithTags("Payments")
        .Produces<IReadOnlyList<MySubscriptionDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
