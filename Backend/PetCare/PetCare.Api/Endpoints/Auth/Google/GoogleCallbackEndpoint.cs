namespace PetCare.Api.Endpoints.Auth.Google;

using MediatR;
using PetCare.Application.Features.Auth.Google.GoogleLogin;

/// <summary>
/// Maps the Google OAuth callback endpoint.
/// </summary>
public static class GoogleCallbackEndpoint
{
    /// <summary>
    /// Integrates the Google callback endpoint into the web application.
    /// </summary>
    /// <param name="app">The web application to integrate the endpoint into.</param>
    public static void MapGoogleCallbackEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/google/callback", async (
            IMediator mediator,
            string code,
            string state,
            HttpContext httpContext,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GoogleCallbackEndpoint");

            try
            {
                var command = new GoogleLoginCallbackCommand(code, state);
                var redirectUrl = await mediator.Send(command);

                logger.LogInformation("Redirecting user to frontend: {Url}", redirectUrl);

                return Results.Redirect(redirectUrl); // Редірект на фронт
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Google login callback failed: {Message}", ex.Message);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("GoogleCallback")
        .WithTags("Auth");
    }
}
