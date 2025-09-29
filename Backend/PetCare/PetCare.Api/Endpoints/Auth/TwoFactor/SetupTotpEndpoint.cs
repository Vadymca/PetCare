namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.SetupTotp;

/// <summary>
/// Contains the endpoint mapping for TOTP two-factor authentication setup.
/// </summary>
public static class SetupTotpEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/2fa/totp/setup endpoint to handle TOTP setup requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapSetupTotpEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/totp/setup", async (
            SetupTotpCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SetupTotpEndpoint");

            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("SetupTotp")
        .WithTags("Auth")
        .Produces<SetupTotpResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<SetupTotpCommand>("application/json");
    }
}
