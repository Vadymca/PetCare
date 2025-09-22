namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.RegenerateBackupCodes;

/// <summary>
/// Maps the POST /api/auth/2fa/totp/regenerate-backup-codes endpoint.
/// </summary>
public static class RegenerateBackupCodesEndpoint
{
    /// <summary>
    /// Adds the endpoint to the <see cref="WebApplication"/>.
    /// Requires the user to be authenticated.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to map the endpoint to.</param>
    public static void MapRegenerateBackupCodesEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/totp/regenerate-backup-codes", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RegenerateBackupCodesEndpoint");

            var command = new RegenerateTotpBackupCodesCommand();
            var result = await mediator.Send(command);

            logger.LogInformation("TOTP backup codes successfully regenerated.");

            return Results.Ok(result);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("RegenerateTotpBackupCodes")
        .WithTags("Auth")
        .Produces<GetTotpBackupCodesResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
