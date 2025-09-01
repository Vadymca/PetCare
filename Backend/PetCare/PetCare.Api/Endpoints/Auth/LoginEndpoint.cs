namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos;
using PetCare.Application.Features.Auth.Login;
using System.Text.Json;

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
        app.MapPost("/api/auth/login", async (HttpContext context, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            try
            {
                var logger = loggerFactory.CreateLogger("LoginEndpoint");

                // Читаємо JSON з body
                using var reader = new StreamReader(context.Request.Body);
                var json = await reader.ReadToEndAsync();
                logger.LogInformation("Received raw JSON: {Json}", json);

                // Десеріалізуємо JSON
                var command = JsonSerializer.Deserialize<LoginUserCommand>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                logger.LogInformation("Deserialized command: Email='{Email}'", command?.Email ?? "NULL");

                var loginResponse = await mediator.Send(command);
                return Results.Ok(loginResponse);
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
        .WithName("Login")
        .WithTags("Auth")
        .Produces<LoginResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<LoginUserCommand>("application/json");
    }
}
