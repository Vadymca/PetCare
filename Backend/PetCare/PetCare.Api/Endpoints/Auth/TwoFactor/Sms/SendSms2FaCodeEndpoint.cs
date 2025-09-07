namespace PetCare.Api.Endpoints.Auth.TwoFactor.Sms;

using MediatR;
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
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SendSms2FaCodeEndpoint");

            try
            {
                var command = new SendSms2FaCodeCommand();
                var result = await mediator.Send(command);

                if (!result.Success)
                {
                    logger.LogWarning("Failed to send SMS 2FA code: {Message}", result.Message);
                    return Results.BadRequest(new { message = result.Message });
                }

                logger.LogInformation("SMS 2FA code successfully sent.");
                return Results.Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending SMS 2FA code");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("SendSms2FaCode")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
