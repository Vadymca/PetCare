namespace PetCare.Api.Endpoints.Auth.TwoFactor.Sms;

using MediatR;
using PetCare.Application.Features.Auth.TwoFactor.Sms.VerifySetup;

/// <summary>
/// Maps the POST /api/auth/2fa/sms/verify-setup endpoint.
/// </summary>
public static class VerifySms2FaSetupEndpoint
{
    /// <summary>
    /// Configures the endpoint for verifying SMS 2FA setup codes.
    /// </summary>
    /// <param name="app">The WebApplication to add the endpoint to.</param>
    public static void MapVerifySms2FaSetupEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/sms/verify-setup", async (
            VerifySms2FaSetupCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("VerifySms2FaSetupEndpoint");

            try
            {
                var result = await mediator.Send(command);

                if (!result.Success)
                {
                    logger.LogWarning("Failed to verify SMS 2FA setup code: {Message}", result.Message);
                    return Results.BadRequest(new { message = result.Message });
                }

                logger.LogInformation("SMS 2FA setup verified successfully.");
                return Results.Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error verifying SMS 2FA setup code");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("VerifySms2FaSetup")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<VerifySms2FaSetupCommand>("application/json");
    }
}
