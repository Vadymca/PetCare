namespace PetCare.Api.Endpoints.Animals;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Features.Animals.UnsubscribeFromAnimal;

/// <summary>
/// Endpoint for unsubscribing the authenticated user from notifications about a specific animal.
/// </summary>
public static class UnsubscribeFromAnimalEndpoint
{
    /// <summary>
    /// Maps the DELETE endpoint for unsubscribing from an animal.
    /// </summary>
    /// , <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapUnsubscribeFromAnimalEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/animals/{id:guid}/subscribe", async (
            Guid id,
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UnsubscribeFromAnimalEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized attempt to unsubscribe from animal {AnimalId}", id);
                return Results.Unauthorized();
            }

            try
            {
                var command = new UnsubscribeFromAnimalCommand(id, userId);
                await mediator.Send(command);

                logger.LogInformation("User {UserId} unsubscribed from animal {AnimalId}", userId, id);

                return Results.Ok(new { Message = "Відписка успішна." });
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error unsubscribing user {UserId} from animal {AnimalId}", userId, id);
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization()
        .WithName("UnsubscribeFromAnimal")
        .WithTags("Animals")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .RequireRateLimiting("GlobalPolicy");
    }
}
