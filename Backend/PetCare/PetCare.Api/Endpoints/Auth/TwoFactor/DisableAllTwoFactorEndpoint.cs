namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.DisableAll;

/// <summary>
/// Maps the POST /api/auth/2fa/disable-all endpoint.
/// </summary>
public static class DisableAllTwoFactorEndpoint
{
    /// <summary>
    /// Configures the endpoint to disable all 2FA methods for the current user.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to add the endpoint to.</param>
    public static void MapDisableAllTwoFactorEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/disable-all", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DisableAllTwoFactorEndpoint");

            logger.LogInformation("Disabling all 2FA methods for current user...");

            var result = await mediator.Send(new DisableAllTwoFactorCommand());

            logger.LogInformation("All 2FA methods successfully disabled.");
            return Results.Ok(result);
        })
       .RequireAuthorization()
       .RequireRateLimiting("GlobalPolicy")
       .WithName("DisableAllTwoFactor")
       .WithTags("Auth")
       .Produces<DisableAllTwoFactorResponseDto>(StatusCodes.Status200OK)
       .Produces(StatusCodes.Status400BadRequest)
       .Produces(StatusCodes.Status500InternalServerError);
    }
}
