namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.GetAnimalSubscriptions;

/// <summary>
/// Endpoint for retrieving all animal subscriptions for a specific user.
/// </summary>
public static class GetAnimalSubscriptionsByUserIdEndpoint
{
    /// <summary>
    /// Maps the GET /api/animals/subscriptions/{userId:guid} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to map the endpoint on.</param>
    public static void MapGetAnimalSubscriptionsByUserIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/animals/subscriptions/{userId:guid}", async (
            Guid userId,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAnimalSubscriptionsByUserIdEndpoint");

            var animals = await mediator.Send(new GetAnimalSubscriptionsCommand(userId));

            logger.LogInformation("Fetched animal subscriptions for user {UserId}", userId);

            return Results.Ok(animals);
        })
        .WithName("GetAnimalSubscriptionsByUserId")
        .WithTags("Animals")
        .Produces<IReadOnlyList<AnimalListDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireRateLimiting("GlobalPolicy");
    }
}
