namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.ResetPassword;

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
        app.MapPost("/api/auth/reset-password", async (ResetPasswordCommand command, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ResetPasswordEndpoint");

            logger.LogInformation("Handling ResetPassword for email: {Email}", command.Email);

            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("ResetPassword")
        .WithTags("Auth")
        .Produces<ResetPasswordResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<ResetPasswordCommand>("application/json");
    }
}
