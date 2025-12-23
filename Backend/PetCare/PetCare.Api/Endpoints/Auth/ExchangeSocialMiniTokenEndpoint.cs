namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Features.Auth.Social.ExchangeMiniToken;

/// <summary>
/// Maps the POST /api/auth/social endpoint.
/// </summary>
public static class ExchangeSocialMiniTokenEndpoint
{
    /// <summary>
    /// Adds the endpoint to the application.
    /// </summary>
    /// <param name="app">WebApplication instance.</param>
    public static void MapExchangeSocialMiniTokenEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/social", async (
            IMediator mediator,
            ExchangeMiniTokenCommand command,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ExchangeSocialMiniTokenEndpoint");
            logger.LogInformation("Mini token exchange requested: {Token}", command.Token);

            try
            {
                await mediator.Send(command);

                logger.LogInformation("Refresh token cookie issued successfully for mini token: {Token}", command.Token);

                // Просто повідомляємо фронту, що все ок
                return Results.Ok(new { message = "ok" });
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning("Mini token exchange failed: {Message}", ex.Message);
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during mini token exchange.");
                return Results.StatusCode(500);
            }
        })
        .WithName("ExchangeSocialMiniToken")
        .RequireRateLimiting("GlobalPolicy")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<ExchangeMiniTokenCommand>("application/json");
    }
}
