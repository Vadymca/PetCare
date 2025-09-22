namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.GetBackupCodes;

/// <summary>
/// Maps the GET /api/auth/2fa/totp/backup-codes endpoint.
/// </summary>
public static class GetTotpBackupCodesEndpoint
{
    /// <summary>
    /// Maps the endpoint for retrieving TOTP backup codes for the currently authenticated user.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to add the endpoint to.</param>
    public static void MapGetTotpBackupCodesEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/2fa/totp/backup-codes", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetTotpBackupCodesEndpoint");

            logger.LogInformation("Retrieving TOTP backup codes for current user.");

            var command = new GetTotpBackupCodesCommand();
            var result = await mediator.Send(command);

            logger.LogInformation("TOTP backup codes successfully retrieved.");
            return Results.Ok(result);
        })
        .WithName("GetTotpBackupCodes")
        .RequireRateLimiting("GlobalPolicy")
        .WithTags("Auth")
        .RequireAuthorization()
        .Produces<GetTotpBackupCodesResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
