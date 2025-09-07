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
        app.MapPost("/api/auth/2fa/disable-all", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DisableAllTwoFactorEndpoint");

            try
            {
                var result = await mediator.Send(new DisableAllTwoFactorCommand());

                if (!result.Success)
                {
                    logger.LogWarning("Failed to disable all 2FA methods: {Message}", result.Message);
                    return Results.BadRequest(new { message = result.Message });
                }

                logger.LogInformation("All 2FA methods successfully disabled.");
                return Results.Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error disabling all 2FA methods");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
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
