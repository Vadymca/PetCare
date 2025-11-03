namespace PetCare.Api.Endpoints.Users;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Users.GetUserById;

/// <summary>
/// Endpoint for retrieving details of a specific user by Id.
/// Accessible only by the resource owner or an Admin.
/// </summary>
public static class GetUserByIdEndpoint
{
    /// <summary>
    /// Maps the GET /api/users/{id} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetUserByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/users/{id}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetUserByIdEndpoint");

            var result = await mediator.Send(new GetUserByIdCommand(id));

            logger.LogInformation("Retrieved details for user {UserId}", id);

            return Results.Ok(result);
        })
        .RequireAuthorization("ResourceOwnerOrAdmin")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetUserById")
        .WithTags("Users")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);
    }
}
