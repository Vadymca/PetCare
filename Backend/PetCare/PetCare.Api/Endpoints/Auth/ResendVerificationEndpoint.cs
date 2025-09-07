namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Features.Auth.ResendVerification;
using System.Text.Json;

/// <summary>
/// Maps the POST /api/auth/resend-verification endpoint.
/// This endpoint allows users to request a new email verification link.
/// </summary>
public static class ResendVerificationEndpoint
{
    /// <summary>
    /// Registers the endpoint in the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to add the endpoint to.</param>
    public static void MapResendVerificationEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/resend-verification", async (HttpContext context, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ResendVerificationEndpoint");

            try
            {
                using var reader = new StreamReader(context.Request.Body);
                var json = await reader.ReadToEndAsync();
                logger.LogInformation("Received JSON: {Json}", json);

                var command = JsonSerializer.Deserialize<ResendVerificationCommand>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                var success = await mediator.Send(command!);
                if (success)
                {
                    return Results.Ok(new { message = "Лист для підтвердження email відправлено." });
                }

                return Results.BadRequest(new { message = "Не вдалося відправити лист для підтвердження." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error resending verification email");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("ResendVerification")
        .WithTags("Auth")
        .Accepts<ResendVerificationCommand>("application/json")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
