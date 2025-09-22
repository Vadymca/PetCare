namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.Logout;

/// <summary>
/// Contains the endpoint mapping for user logout.
/// </summary>
public static class LogoutEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/logout endpoint to handle user logout requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapLogoutEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/logout", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LogoutEndpoint");
            logger.LogInformation("Logout requested.");

            var command = new LogoutUserCommand();
            var result = await mediator.Send(command);

            logger.LogInformation("User successfully logged out.");

            return Results.Ok(result);
        })
         .WithName("Logout")
         .RequireRateLimiting("GlobalPolicy")
         .WithTags("Auth")
         .Produces<LogoutResponseDto>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status500InternalServerError);
    }
}
