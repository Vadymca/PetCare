namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.DisableTotp;

/// <summary>
/// Maps the POST /api/auth/2fa/totp/disable endpoint.
/// </summary>
public static class DisableTotpEndpoint
{
    /// <summary>
    /// Maps the endpoint for disabling TOTP for the currently authenticated user.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to add the endpoint to.</param>
    public static void MapDisableTotpEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/totp/disable", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DisableTotpEndpoint");

            logger.LogInformation("Request received to disable TOTP.");

            var command = new DisableTotpCommand();

            var result = await mediator.Send(command);

            logger.LogInformation("TOTP successfully disabled for current user.");

            return Results.Ok(result);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("DisableTotp")
        .WithTags("Auth")
        .Produces<VerifyTotpResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
