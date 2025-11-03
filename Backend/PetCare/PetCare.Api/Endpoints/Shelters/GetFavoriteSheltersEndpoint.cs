namespace PetCare.Api.Endpoints.Shelters;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.GetFavoriteShelters;

/// <summary>
/// Endpoint for fetching the current user's favorite shelters.
/// </summary>
public static class GetFavoriteSheltersEndpoint
{
    /// <summary>
    /// Maps the GET /api/shelters/favorites endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetFavoriteSheltersEndpoint(this WebApplication app)
    {
        app.MapGet("/api/shelters/favorites", async (
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetFavoriteSheltersEndpoint");

            // Дістаємо Id користувача з токена
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to /api/shelters/favorites");
                return Results.Unauthorized();
            }

            var favoriteShelters = await mediator.Send(new GetFavoriteSheltersCommand(userId));

            logger.LogInformation("Fetched favorite shelters for current user {UserId}", userId);

            return Results.Ok(favoriteShelters);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetFavoriteShelters")
        .WithTags("Shelters")
        .Produces<IReadOnlyList<ShelterListDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
