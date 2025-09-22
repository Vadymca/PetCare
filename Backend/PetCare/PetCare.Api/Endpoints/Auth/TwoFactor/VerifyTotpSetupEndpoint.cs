namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.VerifyTotpSetup;

/// <summary>
/// Contains the endpoint mapping for verifying TOTP two-factor authentication setup.
/// </summary>
public static class VerifyTotpSetupEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/2fa/totp/verify-setup endpoint to handle TOTP verification requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapVerifyTotpSetupEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/totp/verify-setup", async (
            VerifyTotpSetupCommand request,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("VerifyTotpSetupEndpoint");

            var response = await mediator.Send(request);

            logger.LogInformation("TOTP setup successfully verified for user.");

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("VerifyTotpSetup")
        .WithTags("Auth")
        .Produces<VerifyTotpSetupResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<object>("application/json");
    }
}
