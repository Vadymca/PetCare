namespace PetCare.Api.Endpoints.Auth.TwoFactor.Sms;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.Sms.Setup;

/// <summary>
/// Maps the POST /api/auth/2fa/sms/setup endpoint.
/// Responsible for initiating SMS 2FA setup for the current user.
/// </summary>
public static class SetupSms2FaEndpoint
{
    /// <summary>
    /// Registers the SMS 2FA setup endpoint on the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapSetupSms2FaEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/sms/setup", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SetupSms2FaEndpoint");

            var result = await mediator.Send(new SetupSms2FaCommand());

            logger.LogInformation("SMS 2FA setup initiated successfully.");
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("SetupSms2Fa")
        .WithTags("Auth")
        .Produces<SetupSms2FaResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
