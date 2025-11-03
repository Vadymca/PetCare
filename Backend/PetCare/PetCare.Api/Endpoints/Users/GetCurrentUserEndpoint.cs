namespace PetCare.Api.Endpoints.Users;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Users.GetCurrentUser;

/// <summary>
/// Endpoint for fetching the current user's profile.
/// </summary>
public static class GetCurrentUserEndpoint
{
    /// <summary>
    /// Maps the GET /api/users/me endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetCurrentUserEndpoint(this WebApplication app)
    {
        app.MapGet("/api/users/me", async (
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetCurrentUserEndpoint");

            // Дістаємо Id користувача з токена
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to /api/users/me");
                return Results.Unauthorized();
            }

            var userDto = await mediator.Send(new GetCurrentUserCommand(userId));

            logger.LogInformation("Fetched profile for current user {UserId}", userId);

            return Results.Ok(userDto);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetCurrentUser")
        .WithTags("Users")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
