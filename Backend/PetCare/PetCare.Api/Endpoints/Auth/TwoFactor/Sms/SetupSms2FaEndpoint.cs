using MediatR;
using PetCare.Application.Features.Auth.TwoFactor.Sms.Setup;

namespace PetCare.Api.Endpoints.Auth.TwoFactor.Sms
{
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

                try
                {
                    var command = new SetupSms2FaCommand();

                    var result = await mediator.Send(command);

                    if (!result.Success)
                    {
                        logger.LogWarning("Failed to setup SMS 2FA: {Message}", result.Message);
                        return Results.BadRequest(new { message = result.Message });
                    }

                    logger.LogInformation("SMS 2FA setup initiated successfully.");
                    return Results.Ok(new { message = result.Message });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during SMS 2FA setup.");
                    return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .RequireAuthorization()
            .RequireRateLimiting("GlobalPolicy")
            .WithName("SetupSms2Fa")
            .WithTags("Auth")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
