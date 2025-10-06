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
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SetupTotpEndpoint");

            var response = await mediator.Send(new SetupTotpCommand());
            return Results.Ok(response);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("SetupTotp")
        .WithTags("Auth")
        .Produces<SetupTotpResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
