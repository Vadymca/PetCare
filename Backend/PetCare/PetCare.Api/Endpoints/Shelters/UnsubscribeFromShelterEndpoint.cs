namespace PetCare.Api.Endpoints.Shelters;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Features.Shelters.UnsubscribeFromShelter;

/// <summary>
/// Endpoint for unsubscribing the authenticated user from notifications about a specific shelter.
/// </summary>
public static class UnsubscribeFromShelterEndpoint
{
    /// <summary>
    /// Maps the DELETE endpoint for unsubscribing from a shelter.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapUnsubscribeFromShelterEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/shelters/{id:guid}/subscribe", async (
            Guid id,
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UnsubscribeFromShelterEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized attempt to unsubscribe from shelter {ShelterId}", id);
                return Results.Unauthorized();
            }

            try
            {
                var command = new UnsubscribeFromShelterCommand(id, userId);
                var result = await mediator.Send(command);

                logger.LogInformation("User {UserId} unsubscribed from shelter {ShelterId}", userId, id);

                return Results.Ok(new
                {
                    Message = "Відписка успішна.",
                });
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error unsubscribing user {UserId} from shelter {ShelterId}", userId, id);
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("UnsubscribeFromShelter")
        .WithTags("Shelters")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);
    }
}
