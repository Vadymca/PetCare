namespace PetCare.Api.Endpoints.Payments;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.GetMyPaymentHistory;

/// <summary>
/// Endpoint for fetching the current user's payment history.
/// </summary>
public static class GetMyPaymentHistoryEndpoint
{
    /// <summary>
    /// Maps the GET /api/payments/me/history endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetMyPaymentHistoryEndpoint(this WebApplication app)
    {
        app.MapGet("/api/payments/me/history", async (
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetMyPaymentHistoryEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to /api/payments/me/history");
                return Results.Unauthorized();
            }

            var history = await mediator.Send(new GetMyPaymentHistoryCommand(userId));
            logger.LogInformation("Fetched {Count} payments for user {UserId}", history.Count, userId);

            return Results.Ok(history);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetMyPaymentHistory")
        .WithTags("Payments")
        .Produces<IReadOnlyList<MyPaymentHistoryDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
