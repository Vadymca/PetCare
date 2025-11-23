namespace PetCare.Api.Endpoints.Payments.Subscriptions;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Payments.Subscriptions.ResetSubscription;

/// <summary>
/// Provides extension methods for mapping the reset subscription API endpoint to a web application.
/// </summary>
/// <remarks>
/// This endpoint allows the user to cancel their failed or expired subscription
/// and create a new one (new LiqPay recurring contract).
/// The endpoint requires authorization and returns a SubscriptionDto with new providerSubscriptionId.
/// </remarks>
public static class ResetSubscriptionEndpoint
{
    /// <summary>
    /// Maps the endpoint for resetting a subscription (cancel & create new) to the application's request pipeline.
    /// </summary>
    /// <remarks>
    /// POST /api/subscriptions/{id}/reset
    /// Requires authorization.
    /// Accepts body with Amount, Currency, Scope, ScopeId.
    /// Returns 200 OK with the new subscription details.
    /// </remarks>
    /// <param name="app">The <see cref="WebApplication"/> on which the endpoint will be mapped.</param>
    public static void MapResetSubscriptionEndpoint(this WebApplication app)
    {
        app.MapPost("/api/subscriptions/{subscriptionId:guid}/reset", async (
            Guid subscriptionId,
            [FromBody] ResetSubscriptionRequest request,
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("ResetSubscriptionEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                logger.LogWarning(
                    "Unauthorized access attempt to reset subscription {SubId}",
                    subscriptionId);
                return Results.Unauthorized();
            }

            Guid userId = Guid.Parse(userIdClaim);

            try
            {
                var result = await mediator.Send(
                    new ResetSubscriptionCommand(
                        UserId: userId,
                        OldSubscriptionId: subscriptionId,
                        Amount: request.Amount,
                        Currency: request.Currency,
                        Scope: request.Scope,
                        ScopeId: request.ScopeId),
                    cancellationToken);

                logger.LogInformation(
                    "Subscription {SubId} successfully reset for user {UserId}",
                    subscriptionId,
                    userId);

                return Results.Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(
                    ex,
                    "Subscription {SubId} not found for reset.",
                    subscriptionId);

                return Results.NotFound(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(
                    ex,
                    "Invalid operation while resetting subscription {SubId}",
                    subscriptionId);

                return Results.BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Unexpected error resetting subscription {SubId}",
                    subscriptionId);

                return Results.Problem($"Помилка при оновленні підписки: {ex.Message}");
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("ResetSubscription")
        .WithTags("Payments.Subscriptions")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Represents the request body for resetting a subscription.
    /// </summary>
    public sealed record ResetSubscriptionRequest(
        decimal Amount,
        string Currency,
        string Scope,
        Guid? ScopeId);
}
