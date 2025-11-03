namespace PetCare.Api.Endpoints.Users;

using MediatR;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Features.Users.GetUserActivity;

/// <summary>
/// Provides an endpoint for retrieving user activity, including adoption applications and events.
/// </summary>
public static class GetUserActivityEndpoint
{
    /// <summary>
    /// Maps the GET endpoint <c>/api/users/{id}/activity</c> to fetch user activity.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetUserActivityEndpoint(this WebApplication app)
    {
        app.MapGet(
            "/api/users/{id:guid}/activity",
            async (Guid id, IMediator mediator, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
            {
                var logger = loggerFactory.CreateLogger("GetUserActivityEndpoint");
                logger.LogInformation("Fetching activity for user {UserId}", id);

                var result = await mediator.Send(new GetUserActivityCommand(id), cancellationToken);

                logger.LogInformation(
                    "Successfully fetched {ApplicationsCount} applications and {EventsCount} events for user {UserId}",
                    result.AdoptionApplications.Count,
                    result.Events.Count,
                    id);

                return Results.Ok(result);
            })
            .RequireAuthorization("ResourceOwnerOrAdmin")
            .RequireRateLimiting("GlobalPolicy")
            .WithName("GetUserActivity")
            .WithTags("Users")
            .Produces<GetUserActivityResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}
