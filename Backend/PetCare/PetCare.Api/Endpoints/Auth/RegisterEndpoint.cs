namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos;
using PetCare.Application.Features.Auth.Register;

/// <summary>
/// Provides API endpoint mapping for user registration.
/// </summary>
public static class RegisterEndpoint
{
    /// <summary>
    /// Maps the register endpoint (POST /api/auth/register) to the application.
    /// Handles user registration by sending <see cref="RegisterUserCommand"/> to MediatR.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance used to configure the endpoint.</param>
    public static void MapRegisterEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/register", async ([FromBody] RegisterUserCommand command, IMediator mediator) =>
        {
            try
            {
                var userDto = await mediator.Send(command);
                return Results.Created($"/api/users/{userDto.id}", userDto);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("Register")
        .WithTags("Auth")
        .Produces<UserDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
