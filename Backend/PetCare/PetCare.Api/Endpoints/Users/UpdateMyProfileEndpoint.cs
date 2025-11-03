namespace PetCare.Api.Endpoints.Users;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Features.Users.UpdateUser;

/// <summary>
/// Endpoint for updating the authenticated user's profile.
/// </summary>
public static class UpdateMyProfileEndpoint
{
    /// <summary>
    /// Maps the PUT /api/users/me endpoint for updating own profile.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapUpdateMyProfileEndpoint(this WebApplication app)
    {
        app.MapPut("/api/users/me", async (
            UpdateMyProfileCommandBody body,
            IMediator mediator,
            HttpContext httpContext,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UpdateMyProfileEndpoint");

            // Дістаємо Id користувача з токена
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var cmd = new UpdateUserCommand(
                Id: userId,
                Email: body.Email,
                Password: body.Password,
                FirstName: body.FirstName,
                LastName: body.LastName,
                Phone: body.Phone,
                Preferences: body.Preferences,
                Points: null,
                ProfilePhoto: body.ProfilePhoto,
                Language: body.Language,
                PostalCode: body.PostalCode);

            var result = await mediator.Send(cmd);

            logger.LogInformation("User {UserId} updated own profile", userId);

            return Results.Ok(result);
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("UpdateMyProfile")
        .WithTags("Users")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Accepts<UpdateMyProfileCommandBody>("application/json");
    }
}
