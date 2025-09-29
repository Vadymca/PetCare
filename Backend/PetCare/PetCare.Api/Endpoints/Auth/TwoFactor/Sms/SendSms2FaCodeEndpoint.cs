namespace PetCare.Api.Endpoints.Auth.TwoFactor.Sms;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.TwoFactor.Sms.Send;

/// <summary>
/// Maps the POST /api/auth/2fa/sms/send endpoint.
/// </summary>
public static class SendSms2FaCodeEndpoint
{
    /// <summary>
    /// Configures the endpoint for sending SMS 2FA code.
    /// </summary>
    /// <param name="app">The web application to map the endpoint on.</param>
    public static void MapSendSms2FaCodeEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/sms/send", async (
            IMediator mediator,
            SendSms2FaCodeCommand command,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SendSms2FaCodeEndpoint");

            var result = await mediator.Send(command);

            logger.LogInformation("SMS 2FA code successfully sent.");

            return Results.Ok(result);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("SendSms2FaCode")
        .WithTags("Auth")
        .Produces<SendSms2FaCodeResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<SendSms2FaCodeCommand>("application/json");
    }
}
