namespace PetCare.Api.Endpoints.Auth.TwoFactor;

using MediatR;
using PetCare.Application.Features.Auth.TwoFactor.VerifyTotp;
using System.Text.Json;

/// <summary>
/// Maps the POST /api/auth/2fa/totp/verify endpoint.
/// </summary>
public static class VerifyTotpEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/2fa/totp/verify endpoint for verifying TOTP codes during login.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to add the endpoint to.</param>
    public static void MapVerifyTotpEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/2fa/totp/verify", async (HttpContext context, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("VerifyTotpEndpoint");

            try
            {
                using var reader = new StreamReader(context.Request.Body);
                var json = await reader.ReadToEndAsync();
                logger.LogInformation("Received raw JSON: {Json}", json);

                var command = JsonSerializer.Deserialize<VerifyTotpCommand>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (command is null || string.IsNullOrWhiteSpace(command.Code))
                {
                    logger.LogWarning("Missing email or TOTP code in request body.");
                    return Results.BadRequest(new { error = "Email та TOTP код обов'язкові." });
                }

                logger.LogInformation("Deserialized command: Code={Code}", command.Code);

                var result = await mediator.Send(command);

                if (result.Success)
                {
                    logger.LogInformation("TOTP verified successfully");
                    return Results.Ok(result);
                }

                logger.LogWarning("TOTP verification failed");
                return Results.BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while verifying TOTP");
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("VerifyTotp")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<VerifyTotpCommand>("application/json");
    }
}
