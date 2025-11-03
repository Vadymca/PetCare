namespace PetCare.Api.Endpoints.Users;

using MediatR;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Features.Users.GetUserSubscriptions;

/// <summary>
/// Provides an endpoint for retrieving user subscriptions (shelters and animals).
/// </summary>
public static class GetUserSubscriptionsEndpoint
{
    /// <summary>
    /// Maps the GET endpoint <c>/api/users/{id}/subscriptions</c> to fetch user subscriptions.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    public static void MapGetUserSubscriptionsEndpoint(this WebApplication app)
    {
        app.MapGet(
            "/api/users/{id:guid}/subscriptions",
            async (Guid id, IMediator mediator, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
            {
                var logger = loggerFactory.CreateLogger("GetUserSubscriptionsEndpoint");

                logger.LogInformation("Fetching subscriptions for user {UserId}", id);

                var result = await mediator.Send(new GetUserSubscriptionsCommand(id), cancellationToken);

                logger.LogInformation(
                    "Successfully fetched {ShelterCount} shelters and {AnimalCount} animals for user {UserId}",
                    result.Shelters.Count,
                    result.Animals.Count,
                    id);

                return Results.Ok(result);
            })
            .RequireAuthorization("ResourceOwnerOrAdmin")
            .RequireRateLimiting("GlobalPolicy")
            .WithName("GetUserSubscriptions")
            .WithTags("Users")
            .Produces<GetUserSubscriptionsResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}
