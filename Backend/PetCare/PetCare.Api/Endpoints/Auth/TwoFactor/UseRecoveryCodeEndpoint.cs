namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes.Use;

/// <summary>
/// Maps the POST /api/auth/2fa/use-recovery-code endpoint.
/// </summary>
public static class UseRecoveryCodeEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/2fa/use-recovery-code endpoint.
    /// This endpoint allows the authenticated user to redeem a TOTP recovery code
    /// for two-factor authentication (2FA) in case they cannot use their primary 2FA method.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to map the endpoint on.</param>
    public static void MapUseRecoveryCodeEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/use-recovery-code", async (
            UseRecoveryCodeCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UseRecoveryCodeEndpoint");

            logger.LogInformation("Recovery code successfully used.");

            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("UseRecoveryCode")
        .WithTags("Auth")
        .Produces<UseRecoveryCodeResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<UseRecoveryCodeCommand>("application/json");
    }
}
