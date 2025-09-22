namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Auth.Refresh;

/// <summary>
/// Contains the endpoint mapping for refreshing JWT tokens.
/// </summary>
public static class RefreshEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/refresh endpoint to handle JWT refresh requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapRefreshEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/refresh", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RefreshEndpoint");
            logger.LogInformation("Token refresh requested.");

            var response = await mediator.Send(new RefreshUserCommand());

            logger.LogInformation("Token refresh successful.");

            return Results.Ok(response);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("Refresh")
        .WithTags("Auth")
        .Produces<LoginResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
