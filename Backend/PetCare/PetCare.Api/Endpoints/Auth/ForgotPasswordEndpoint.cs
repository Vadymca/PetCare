namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using Microsoft.AspNetCore.Http;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.ForgotPassword;
using System.Text.Json;

/// <summary>
/// Contains the endpoint mapping for forgot password.
/// </summary>
public static class ForgotPasswordEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/forgot-password endpoint to handle forgot password requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapForgotPasswordEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/forgot-password", async (HttpContext context, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            try
            {
                var logger = loggerFactory.CreateLogger("ForgotPasswordEndpoint");

                // Read raw JSON from request body
                using var reader = new StreamReader(context.Request.Body);
                var json = await reader.ReadToEndAsync();
                logger.LogInformation("Received raw JSON: {Json}", json);

                // Deserialize into command
                var command = JsonSerializer.Deserialize<ForgotPasswordCommand>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                logger.LogInformation("Deserialized command: Email='{Email}'", command?.Email ?? "NULL");

                var response = await mediator.Send(command!);
                return Results.Ok(response);
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
        .WithName("ForgotPassword")
        .RequireRateLimiting("GlobalPolicy")
        .WithTags("Auth")
        .Produces<ForgotPasswordResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<ForgotPasswordCommand>("application/json");
    }
}
