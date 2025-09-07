namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes;

/// <summary>
/// Maps the GET /api/auth/2fa/recovery-codes endpoint.
/// </summary>
public static class RecoveryCodesEndpoint
{
    /// <summary>
    /// Defines and maps the GET <c>/api/auth/2fa/recovery-codes</c> endpoint.
    /// This endpoint retrieves recovery codes for the currently authenticated user.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance used to configure the endpoint.</param>
    public static void MapRecoveryCodesEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/2fa/recovery-codes", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RecoveryCodesEndpoint");

            try
            {
                var result = await mediator.Send(new GetRecoveryCodesCommand());
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving recovery codes");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetRecoveryCodes")
        .WithTags("Auth")
        .Produces<RecoveryCodesResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
