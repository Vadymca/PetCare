namespace PetCare.Api.Endpoints.Auth.TwoFactor.Sms;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
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

            logger.LogInformation("Attempting to verify SMS 2FA code.");
            var result = await mediator.Send(command);

            logger.LogInformation("SMS 2FA code verified successfully for user {UserId}", result.User?.Id);
            return Results.Ok(result);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("VerifySms2FaCode")
        .WithTags("Auth")
        .Produces<VerifySms2FaCodeResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<VerifySms2FaCodeCommand>("application/json");
    }
}
