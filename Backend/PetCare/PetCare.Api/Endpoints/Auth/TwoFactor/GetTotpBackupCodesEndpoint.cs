namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
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

            try
            {
                var command = new GetTotpBackupCodesCommand();

                var result = await mediator.Send(command);

                if (!result.Success)
                {
                    logger.LogWarning("Failed to retrieve TOTP backup codes: {Message}", result.Message);
                    return Results.BadRequest(new { message = result.Message });
                }

                logger.LogInformation("TOTP backup codes successfully retrieved.");
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving TOTP backup codes.");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("GetTotpBackupCodes")
        .WithTags("Auth")
        .RequireAuthorization()
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
