namespace PetCare.Api.Endpoints.Auth.TwoFactor.Sms;

using MediatR;
using PetCare.Application.Features.Auth.TwoFactor.Sms.Verify;

/// <summary>
/// Maps the POST /api/auth/2fa/sms/verify endpoint.
/// </summary>
public static class VerifySms2FaCodeEndpoint
{
    /// <summary>
    /// Adds the endpoint mapping for verifying SMS 2FA codes.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapVerifySms2FaCodeEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/sms/verify", async (
            VerifySms2FaCodeCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("VerifySms2FaCodeEndpoint");

            try
            {
                var result = await mediator.Send(command);

                if (!result.Success)
                {
                    logger.LogWarning("Failed to verify SMS 2FA code: {Message}", result.Message);
                    return Results.BadRequest(new { message = result.Message });
                }

                logger.LogInformation("SMS 2FA code verified successfully.");
                return Results.Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error verifying SMS 2FA code");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("VerifySms2FaCode")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<VerifySms2FaCodeCommand>("application/json");
    }
}
