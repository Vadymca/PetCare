namespace PetCare.Api.Endpoints.Users;

using MediatR;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Features.Users.DeleteUser;

/// <summary>
/// Endpoint for deleting a user (Admin).
/// </summary>
public static class DeleteUserEndpoint
{
    /// <summary>
    /// Maps the DELETE /api/users/{id} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapDeleteUserEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/users/{id}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DeleteUserEndpoint");

            var cmd = new DeleteUserCommand(id);
            var result = await mediator.Send(cmd);

            logger.LogInformation("Admin deleted user {UserId}", id);

            return Results.Ok(result);
        })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("DeleteUser")
        .WithTags("Users")
        .Produces<DeleteUserResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);
    }
}
