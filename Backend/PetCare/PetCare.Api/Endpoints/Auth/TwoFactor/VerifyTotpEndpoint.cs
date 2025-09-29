namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.VerifyTotp;

/// <summary>
/// Maps the POST /api/auth/2fa/totp/verify endpoint.
/// </summary>
public static class VerifyTotpEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/2fa/totp/verify endpoint for verifying TOTP codes during login.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to add the endpoint to.</param>
    public static void MapVerifyTotpEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/totp/verify", async (VerifyTotpCommand command, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("VerifyTotpEndpoint");

            var result = await mediator.Send(command);

            logger.LogInformation("TOTP verified successfully");
            return Results.Ok(result);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("VerifyTotp")
        .WithTags("Auth")
        .Produces<VerifyTotpResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<VerifyTotpCommand>("application/json");
    }
}
