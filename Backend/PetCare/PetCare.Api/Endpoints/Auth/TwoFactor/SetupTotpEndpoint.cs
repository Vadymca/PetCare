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
        app.MapPost("/api/auth/2fa/totp/setup", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SetupTotpEndpoint");

            try
            {
                var response = await mediator.Send(new SetupTotpCommand());
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while setting up TOTP.");
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .WithName("SetupTotp")
        .WithTags("Auth")
        .Produces<SetupTotpResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
