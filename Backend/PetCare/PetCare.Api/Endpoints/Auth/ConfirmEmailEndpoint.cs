namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Features.Auth.ConfirmEmail;
using System.IO;
using System.Text.Json;

/// <summary>
/// Maps the POST /api/auth/confirm-email endpoint.
/// </summary>
public static class ConfirmEmailEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/confirm-email endpoint for confirming a user's email.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to add the endpoint to.</param>
    public static void MapConfirmEmailEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/confirm-email", async (HttpContext context, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ConfirmEmailEndpoint");

            try
            {
                using var reader = new StreamReader(context.Request.Body);
                var json = await reader.ReadToEndAsync();
                logger.LogInformation("Received raw JSON: {Json}", json);

                var command = JsonSerializer.Deserialize<ConfirmEmailCommand>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (command is null || string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Token))
                {
                    logger.LogWarning("Missing email or token in request body.");
                    return Results.BadRequest(new { error = "Email та token обов'язкові." });
                }

                logger.LogInformation("Deserialized command: Email={Email}, Token={Token}", command.Email, command.Token);

                var success = await mediator.Send(command);

                if (success)
                {
                    logger.LogInformation("Email successfully confirmed for {Email}", command.Email);
                    return Results.Ok(new { message = "Email успішно підтверджений." });
                }

                logger.LogWarning("Failed to confirm email for {Email}", command.Email);
                return Results.BadRequest(new { message = "Не вдалося підтвердити email." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while confirming email");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("ConfirmEmail")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<ConfirmEmailCommand>("application/json");
    }
}
