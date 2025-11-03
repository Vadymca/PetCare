namespace PetCare.Api.Endpoints.Users;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Users.UpdateUser;

/// <summary>
/// Endpoint for updating a user (Admin).
/// </summary>
public static class UpdateUserEndpoint
{
    /// <summary>
    /// Maps the PUT /api/users/{id} endpoint for updating a user.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapUpdateUserEndpoint(this WebApplication app)
    {
        app.MapPut("/api/users/{id}", async (
            Guid id,
            UpdateUserCommandBody body,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UpdateUserEndpoint");

            var cmd = new UpdateUserCommand(
                Id: id,
                Email: body.Email,
                Password: body.Password,
                FirstName: body.FirstName,
                LastName: body.LastName,
                Phone: body.Phone,
                Preferences: body.Preferences,
                Points: body.Points,
                ProfilePhoto: body.ProfilePhoto,
                Language: body.Language,
                PostalCode: body.PostalCode);

            var result = await mediator.Send(cmd);

            logger.LogInformation("Admin updated user {UserId}", id);

            return Results.Ok(result);
        })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("UpdateUser")
        .WithTags("Users")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Accepts<UpdateUserCommandBody>("application/json");
    }
}