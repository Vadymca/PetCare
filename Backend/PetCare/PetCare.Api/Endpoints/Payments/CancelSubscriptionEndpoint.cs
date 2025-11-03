namespace PetCare.Api.Endpoints.Payments;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Features.Payments.CancelSubscription;

/// <summary>
/// Provides extension methods for mapping the cancel subscription API endpoint to a web application.
/// </summary>
/// <remarks>This static class contains methods for configuring the endpoint that handles subscription
/// cancellation requests. The endpoint requires authorization and responds with appropriate status codes based on the
/// outcome of the cancellation operation, including success, unauthorized access, not found, and internal server
/// errors.</remarks>
public static class CancelSubscriptionEndpoint
{
    /// <summary>
    /// Maps the endpoint for canceling a subscription to the application's request pipeline. The endpoint handles HTTP
    /// POST requests to '/api/subscriptions/{providerSubscriptionId}/cancel' and processes subscription cancellation
    /// requests.
    /// </summary>
    /// <remarks>The mapped endpoint requires authorization and returns appropriate HTTP status codes based on
    /// the outcome of the cancellation request, including 200 OK for successful cancellation, 401 Unauthorized if the
    /// user is not authenticated, 404 Not Found if the subscription does not exist, and 500 Internal Server Error for
    /// unexpected errors. The endpoint is tagged as 'Payments' and named 'CancelSubscription'.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the cancel subscription endpoint will be mapped.</param>
    public static void MapCancelSubscriptionEndpoint(this WebApplication app)
    {
        app.MapPost("/api/subscriptions/{providerSubscriptionId}/cancel", async (
            string providerSubscriptionId,
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("CancelSubscriptionEndpoint");
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                logger.LogWarning("Unauthorized access attempt to cancel subscription {ProviderSubscriptionId}", providerSubscriptionId);
                return Results.Unauthorized();
            }

            try
            {
                await mediator.Send(new CancelSubscriptionCommand(providerSubscriptionId), cancellationToken);
                logger.LogInformation("Subscription {ProviderSubscriptionId} cancelled successfully.", providerSubscriptionId);
                return Results.Ok(new { Message = "Підписку успішно скасовано." });
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, "Subscription {ProviderSubscriptionId} not found.", providerSubscriptionId);
                return Results.NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error cancelling subscription {ProviderSubscriptionId}", providerSubscriptionId);
                return Results.Problem($"Помилка при скасуванні підписки: {ex.Message}");
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("CancelSubscription")
        .WithTags("Payments")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
