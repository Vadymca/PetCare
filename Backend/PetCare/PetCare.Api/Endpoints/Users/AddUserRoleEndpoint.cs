namespace PetCare.Api.Endpoints.Users;

using MediatR;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Features.Users.Roles;

/// <summary>
/// Endpoint for adding a role to a user.
/// </summary>
public static class AddUserRoleEndpoint
{
    /// <summary>
    /// Maps the POST endpoint for adding a role to a specific user.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapAddUserRoleEndpoint(this WebApplication app)
    {
        app.MapPost("/api/users/{id}/roles", async (
            Guid id,
            AddUserRoleCommandBody body,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AddUserRoleEndpoint");

            // Формуємо команду з правильним UserId
            var cmd = new AddUserRoleCommand(id, body.Role);
            var result = await mediator.Send(cmd);

            logger.LogInformation("Role {Role} successfully added to user {UserId}", cmd.Role, id);

            return Results.Ok(result);
        })
        .RequireAuthorization("AdminOnly") // політика AdminOnly
        .RequireRateLimiting("GlobalPolicy")
        .WithName("AddUserRole")
        .WithTags("Users")
        .Produces<AddUserRoleResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<AddUserRoleCommandBody>("application/json");
    }
}
