namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.VerifyTotpBackupCode;

/// <summary>
/// Maps the POST /api/auth/2fa/totp/verify-backup-code endpoint.
/// </summary>
public static class VerifyTotpBackupCodeEndpoint
{
    /// <summary>
    /// Adds the endpoint for verifying a TOTP backup code to the WebApplication.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoint is mapped.</param>
    public static void MapVerifyTotpBackupCodeEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/totp/verify-backup-code", async (VerifyTotpBackupCodeCommand command, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("VerifyTotpBackupCodeEndpoint");

            var result = await mediator.Send(command);

            logger.LogInformation("TOTP backup code successfully verified.");
            return Results.Ok(result);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("VerifyTotpBackupCode")
        .WithTags("Auth")
        .Produces<VerifyTotpResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<VerifyTotpBackupCodeCommand>("application/json");
    }
}
