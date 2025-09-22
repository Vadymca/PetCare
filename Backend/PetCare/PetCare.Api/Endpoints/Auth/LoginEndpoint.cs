namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.Login;

/// <summary>
/// Contains the endpoint mapping for user login.
/// </summary>
public static class LoginEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/login endpoint to handle user login requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapLoginEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/login", async (IMediator mediator, LoginUserCommand command, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LoginEndpoint");
            logger.LogInformation("Login attempt for email: {Email}", command.Email);

            var loginResponse = await mediator.Send(command);

            logger.LogInformation("Login result for email {Email}: {Status}", command.Email, loginResponse.Status);

            return Results.Ok(loginResponse);
        })
         .RequireRateLimiting("GlobalPolicy")
         .WithName("Login")
         .WithTags("Auth")
         .Produces<LoginResponseDto>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest)
         .Produces(StatusCodes.Status500InternalServerError)
         .Accepts<LoginUserCommand>("application/json");
    }
}
