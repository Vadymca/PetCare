namespace PetCare.Api.Endpoints.Users;

using MediatR;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Features.Users.GetUsers;

/// <summary>
/// Endpoint for retrieving a paginated list of users with optional filters.
/// Only accessible by Admin users.
/// </summary>
public static class GetUsersEndpoint
{
    /// <summary>
    /// Maps the GET /api/users endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetUsersEndpoint(this WebApplication app)
    {
        app.MapGet("/api/users", async (
            [AsParameters] GetUsersCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetUsersEndpoint");

            var result = await mediator.Send(command);

            logger.LogInformation(
                "Retrieved {Count} users (Page {Page}, PageSize {PageSize}) with search='{Search}' and role='{Role}'",
                result.Users.Count,
                command.Page,
                command.PageSize,
                command.Search,
                command.Role);

            return Results.Ok(result);
        })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetUsers")
        .WithTags("Users")
        .Produces<GetUsersResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
