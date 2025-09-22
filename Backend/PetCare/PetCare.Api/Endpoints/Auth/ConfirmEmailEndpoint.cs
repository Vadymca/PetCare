namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.ConfirmEmail;

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
        app.MapPost("/api/auth/confirm-email", async (IMediator mediator, ConfirmEmailCommand command, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ConfirmEmailEndpoint");
            logger.LogInformation("Confirm email request for: {Email}", command.Email);

            var response = await mediator.Send(command);

            if (response.Success)
            {
                logger.LogInformation("Email confirmed for {Email}", command.Email);
                return Results.Ok(response);
            }

            logger.LogWarning("Email confirmation failed for {Email}: {Message}", command.Email, response.Message);
            return Results.BadRequest(response);
        })
         .WithName("ConfirmEmail")
         .RequireRateLimiting("GlobalPolicy")
         .WithTags("Auth")
         .Produces<ConfirmEmailResponseDto>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest)
         .Produces(StatusCodes.Status500InternalServerError)
         .Accepts<ConfirmEmailCommand>("application/json");
    }
}
