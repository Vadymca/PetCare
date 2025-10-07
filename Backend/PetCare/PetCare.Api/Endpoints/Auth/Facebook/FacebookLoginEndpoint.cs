namespace PetCare.Api.Endpoints.Auth.Facebook;

using MediatR;
using PetCare.Application.Features.Auth.Facebook.GetFacebookLoginUrl;

/// <summary>
/// Contains the endpoint mapping for Facebook authentication.
/// </summary>
public static class FacebookLoginEndpoint
{
    /// <summary>
    /// Maps the GET /api/auth/facebook endpoint to initiate Facebook login flow.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapFacebookLoginEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/facebook", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("FacebookLoginEndpoint");
            var state = Guid.NewGuid().ToString();

            logger.LogInformation("Ініціалізація входу через Facebook. State: {State}", state);

            var loginUrl = await mediator.Send(new GetFacebookLoginUrlCommand(state));

            logger.LogInformation("Згенеровано URL для входу через Facebook");

            return Results.Redirect(loginUrl);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("FacebookLogin")
        .WithTags("Auth")
        .Produces(StatusCodes.Status302Found)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
