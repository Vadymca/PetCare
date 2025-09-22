namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.Register;

/// <summary>
/// Contains endpoint mapping for user registration.
/// </summary>
public static class RegisterEndpoint
{
    /// <summary>
    /// Maps the HTTP POST /api/auth/register endpoint for registering new users.
    /// Handles JSON input, deserializes into <see cref="RegisterUserCommand"/>,
    /// sends the command via MediatR, and returns appropriate HTTP responses.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to which the endpoint will be added.</param>
    public static void MapRegisterEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/register", async (IMediator mediator, RegisterUserCommand command, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RegisterEndpoint");
            logger.LogInformation("Registration requested for email: {Email}", command.Email);

            var userDto = await mediator.Send(command);

            logger.LogInformation("Registration successful for email: {Email}, UserId: {UserId}", command.Email, userDto.Id);

            return Results.Created($"/api/users/{userDto.Id}", userDto);
        })
         .RequireRateLimiting("GlobalPolicy")
         .WithName("Register")
         .WithTags("Auth")
         .Produces<UserDto>(StatusCodes.Status201Created)
         .Produces(StatusCodes.Status400BadRequest)
         .Produces(StatusCodes.Status500InternalServerError)
         .Accepts<RegisterUserCommand>("application/json");
    }
}
