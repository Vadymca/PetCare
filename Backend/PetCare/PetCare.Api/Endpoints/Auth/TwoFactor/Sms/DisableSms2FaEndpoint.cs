namespace PetCare.Api.Endpoints.Auth.TwoFactor.Sms;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.Sms.Disable;

/// <summary>
/// Maps the POST /api/auth/2fa/sms/disable endpoint.
/// </summary>
public static class DisableSms2FaEndpoint
{
    /// <summary>
    /// Registers the endpoint in the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The web application to map the endpoint to.</param>
    public static void MapDisableSms2FaEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/sms/disable", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DisableSms2FaEndpoint");

            var command = new DisableSms2FaCommand();
            var result = await mediator.Send(command);

            logger.LogInformation("SMS 2FA successfully disabled.");
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("DisableSms2Fa")
        .WithTags("Auth")
        .Produces<DisableSms2FaResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
