namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
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

            try
            {
                var command = new DisableTotpCommand();

                var result = await mediator.Send(command);

                if (!result.Success)
                {
                    logger.LogWarning("Failed to disable TOTP: {Message}", result.Message);
                    return Results.BadRequest(new { message = result.Message });
                }

                logger.LogInformation("TOTP successfully disabled.");
                return Results.Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error disabling TOTP");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("DisableTotp")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
