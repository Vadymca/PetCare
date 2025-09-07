namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.ResetPassword;
using System.Text.Json;

/// <summary>
/// Maps the POST /api/auth/reset-password endpoint.
/// </summary>
public static class ResetPasswordEndpoint
{
    /// <summary>
    /// Registers the endpoint in the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to add the endpoint to.</param>
    public static void MapResetPasswordEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/reset-password", async (HttpContext context, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ResetPasswordEndpoint");
            try
            {
                using var reader = new StreamReader(context.Request.Body);
                var json = await reader.ReadToEndAsync();
                logger.LogInformation("Received raw JSON: {Json}", json);

                var command = JsonSerializer.Deserialize<ResetPasswordCommand>(json, new JsonSerializerOptions
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
        .WithName("ResetPassword")
        .WithTags("Auth")
        .Produces<ResetPasswordResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<ResetPasswordCommand>("application/json");
    }
}
