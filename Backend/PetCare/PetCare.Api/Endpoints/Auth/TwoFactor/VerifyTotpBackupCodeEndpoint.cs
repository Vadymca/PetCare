namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
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

            try
            {
                var result = await mediator.Send(command);

                if (!result.Success)
                {
                    logger.LogWarning("Failed to verify TOTP backup code: {Message}", result.Message);
                    return Results.BadRequest(new { message = result.Message });
                }

                logger.LogInformation("TOTP backup code successfully verified.");
                return Results.Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error verifying TOTP backup code");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("VerifyTotpBackupCode")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<VerifyTotpBackupCodeCommand>("application/json");
    }
}
