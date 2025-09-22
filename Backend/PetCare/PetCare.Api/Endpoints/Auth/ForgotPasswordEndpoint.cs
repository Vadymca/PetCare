namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using Microsoft.AspNetCore.Http;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.ForgotPassword;

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
        app.MapPost("/api/auth/forgot-password", async (IMediator mediator, ForgotPasswordCommand command, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ForgotPasswordEndpoint");
            logger.LogInformation("Forgot password requested for email: {Email}", command.Email);

            var response = await mediator.Send(command);

            logger.LogInformation("Forgot password processed for email: {Email}", command.Email);
            return Results.Ok(response);
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
