namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.Status;

/// <summary>
/// Maps the GET /api/auth/2fa/status endpoint.
/// </summary>
public static class TwoFactorStatusEndpoint
{
    /// <summary>
    /// Configures the endpoint for retrieving the 2FA status.
    /// </summary>
    /// <param name="app">The WebApplication instance to map the endpoint to.</param>
    public static void MapTwoFactorStatusEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/2fa/status", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("TwoFactorStatusEndpoint");

            try
            {
                var result = await mediator.Send(new GetTwoFactorStatusQuery());
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving 2FA status");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetTwoFactorStatus")
        .WithTags("Auth")
        .Produces<TwoFactorStatusResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
