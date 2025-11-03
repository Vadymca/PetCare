namespace PetCare.Api.Endpoints.Animals;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.GetFavoriteAnimals;

/// <summary>
/// Endpoint for fetching the current user's favorite animals.
/// </summary>
public static class GetFavoriteAnimalsEndpoint
{
    /// <summary>
    /// Maps the GET /api/users/me/favorites endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetFavoriteAnimalsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/animals/favorites", async (
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetFavoriteAnimalsEndpoint");

            // Дістаємо Id користувача з токена
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to /api/users/me/favorites");
                return Results.Unauthorized();
            }

            var favoriteAnimals = await mediator.Send(new GetFavoriteAnimalsCommand(userId));

            logger.LogInformation("Fetched favorite animals for current user {UserId}", userId);

            return Results.Ok(favoriteAnimals);
        })
        .RequireAuthorization()
        .WithName("GetFavoriteAnimals")
        .WithTags("Animals")
        .Produces<IReadOnlyList<AnimalListDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireRateLimiting("GlobalPolicy");
    }
}
