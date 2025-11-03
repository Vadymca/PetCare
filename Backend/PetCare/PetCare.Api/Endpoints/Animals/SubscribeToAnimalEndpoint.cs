namespace PetCare.Api.Endpoints.Animals;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.SubscribeToAnimal;

/// <summary>
/// Endpoint for subscribing the current user to an animal's updates.
/// </summary>
public static class SubscribeToAnimalEndpoint
{
    /// <summary>
    /// Maps the subscribe to animal endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapSubscribeToAnimalEndpoint(this WebApplication app)
    {
        app.MapPost("/api/animals/{id:guid}/subscribe", async (
            Guid id,
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SubscribeToAnimalEndpoint");

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized attempt to subscribe to animal {AnimalId}", id);
                return Results.Unauthorized();
            }

            try
            {
                var command = new SubscribeToAnimalCommand(id, userId);
                var subscription = await mediator.Send(command);

                logger.LogInformation("User {UserId} subscribed to animal {AnimalId}", userId, id);

                return Results.Ok(subscription);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error subscribing user {UserId} to animal {AnimalId}", userId, id);
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization()
        .WithName("SubscribeToAnimal")
        .WithTags("Animals")
        .Produces<AnimalSubscriptionDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .RequireRateLimiting("GlobalPolicy");
    }
}
