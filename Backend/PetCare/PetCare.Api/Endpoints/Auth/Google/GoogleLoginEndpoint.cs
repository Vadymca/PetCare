namespace PetCare.Api.Endpoints.Auth.Google;

using MediatR;
using PetCare.Application.Features.Auth.Google.GetGoogleLoginUrl;

/// <summary>
/// Contains the endpoint mapping for Google authentication.
/// </summary>
public static class GoogleLoginEndpoint
{
    /// <summary>
    /// Maps the GET /api/auth/google endpoint to initiate Google login flow.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapGoogleLoginEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/google", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GoogleLoginEndpoint");
            var state = Guid.NewGuid().ToString();

            logger.LogInformation("Ініціалізація входу через Google. State: {State}", state);

            var loginUrl = await mediator.Send(new GetGoogleLoginUrlCommand(state));

            logger.LogInformation("Згенеровано URL для входу через Google");

            return Results.Redirect(loginUrl);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GoogleLogin")
        .WithTags("Auth")
        .Produces(StatusCodes.Status302Found)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
