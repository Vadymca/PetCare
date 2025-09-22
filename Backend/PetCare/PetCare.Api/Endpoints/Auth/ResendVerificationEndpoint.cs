namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.ResendVerification;

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
        app.MapPost("/api/auth/resend-verification", async (ResendVerificationCommand command, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ResendVerificationEndpoint");
            logger.LogInformation("Resend verification requested for email: {Email}", command.Email);

            var response = await mediator.Send(command);

            return Results.Ok(response);
        })
       .RequireRateLimiting("GlobalPolicy")
       .WithName("ResendVerification")
       .WithTags("Auth")
       .Accepts<ResendVerificationCommand>("application/json")
       .Produces<ResendVerificationResponseDto>(StatusCodes.Status200OK)
       .Produces(StatusCodes.Status400BadRequest)
       .Produces(StatusCodes.Status500InternalServerError);
    }
}
