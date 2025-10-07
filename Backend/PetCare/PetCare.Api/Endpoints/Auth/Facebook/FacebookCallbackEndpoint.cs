namespace PetCare.Api.Endpoints.Auth.Facebook;

using MediatR;
using PetCare.Application.Features.Auth.Facebook.FacebookLogin;

/// <summary>
/// Maps the Facebook callback endpoint.
/// </summary>
public static class FacebookCallbackEndpoint
{
    /// <summary>
    /// Integrates the Facebook callback endpoint into the web application.
    /// </summary>
    /// <param name="app">The web application to integrate the endpoint into.</param>
    public static void MapFacebookCallbackEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/facebook/callback", async (
        IMediator mediator,
        string code,
        string state,
        HttpContext httpContext,
        ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("FacebookCallbackEndpoint");

            try
            {
                var command = new FacebookLoginCallbackCommand(code, state);
                var redirectUrl = await mediator.Send(command);

                logger.LogInformation("Redirecting user to frontend: {Url}", redirectUrl);

                return Results.Redirect(redirectUrl); // Редірект на фронт
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Facebook login callback failed: {Message}", ex.Message);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("FacebookCallback")
        .WithTags("Auth");
    }
}
