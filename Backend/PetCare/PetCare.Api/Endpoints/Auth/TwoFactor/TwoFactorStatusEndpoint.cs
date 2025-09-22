namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.Status;

/// <summary>
/// Maps the GET /api/auth/2fa/status endpoint.
/// </summary>
public static class TwoFactorStatusEndpoint
{
    /// <summary>
    /// Configures the endpoint for retrieving the 2FA status.
    /// </summary>
    /// <param name="app">The WebApplication instance to map the endpoint to.</param>
    public static void MapTwoFactorStatusEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/2fa/status", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("TwoFactorStatusEndpoint");

            var result = await mediator.Send(new GetTwoFactorStatusQuery());

            logger.LogInformation("2FA status retrieved successfully for current user.");

            return Results.Ok(result);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetTwoFactorStatus")
        .WithTags("Auth")
        .Produces<TwoFactorStatusResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
