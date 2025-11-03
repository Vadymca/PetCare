namespace PetCare.Api.Endpoints.Shelters;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.SubscribeToShelter;

/// <summary>
/// Endpoint for subscribing the current user to a shelter's updates.
/// </summary>
public static class SubscribeToShelterEndpoint
{
    /// <summary>
    /// Maps the POST /api/shelters/{id}/subscribe endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapSubscribeToShelterEndpoint(this WebApplication app)
    {
        app.MapPost("/api/shelters/{id:guid}/subscribe", async (
            Guid id,
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SubscribeToShelterEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized attempt to subscribe to shelter {ShelterId}", id);
                return Results.Unauthorized();
            }

            try
            {
                var command = new SubscribeToShelterCommand(id, userId);
                var subscription = await mediator.Send(command);

                logger.LogInformation("User {UserId} subscribed to shelter {ShelterId}", userId, id);

                return Results.Ok(subscription);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error subscribing user {UserId} to shelter {ShelterId}", userId, id);
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("SubscribeToShelter")
        .WithTags("Shelters")
        .Produces<ShelterSubscriptionDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);
    }
}
