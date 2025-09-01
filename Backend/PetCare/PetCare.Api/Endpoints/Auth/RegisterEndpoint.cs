namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos;
using PetCare.Application.Features.Auth.Register;
using System.Text.Json;

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
        app.MapPost("/api/auth/register", async (HttpContext context, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RegisterEndpoint");

            try
            {
                // Читаємо JSON з body
                using var reader = new StreamReader(context.Request.Body);
                var json = await reader.ReadToEndAsync();
                logger.LogInformation("Received raw JSON: {Json}", json);

                // Десеріалізуємо JSON
                var command = JsonSerializer.Deserialize<RegisterUserCommand>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                logger.LogInformation(
                    "Deserialized command: Email='{Email}', FirstName='{FirstName}', LastName='{LastName}', PhoneNumber='{PhoneNumber}', PostalCode='{PostalCode}'",
                    command?.Email ?? "NULL",
                    command?.FirstName ?? "NULL",
                    command?.LastName ?? "NULL",
                    command?.PhoneNumber ?? "NULL",
                    command?.PostalCode ?? "NULL");

                var userDto = await mediator.Send(command);
                return Results.Created($"/api/users/{userDto.Id}", userDto);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "Registration failed");
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during registration");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("Register")
        .WithTags("Auth")
        .Produces<UserDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<RegisterUserCommand>("application/json");
    }
}
