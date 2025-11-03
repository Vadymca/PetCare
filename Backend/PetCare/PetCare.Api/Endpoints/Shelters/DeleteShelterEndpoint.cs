namespace PetCare.Api.Endpoints.Shelters;

using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.DeleteShelter;

/// <summary>
/// Provides extension methods for configuring the endpoint that deletes a shelter resource via the API.
/// </summary>
/// <remarks>This class contains static methods for mapping the delete shelter endpoint to a WebApplication
/// instance. The endpoint requires 'AdminOnly' authorization and responds with appropriate status codes for success,
/// unauthorized access, not found, and internal server errors. Use these methods during application startup to enable
/// shelter deletion functionality in the API.</remarks>
public static class DeleteShelterEndpoint
{
    /// <summary>
    /// Maps the HTTP DELETE endpoint for removing a shelter by its unique identifier to the specified web application.
    /// The endpoint requires 'AdminOnly' authorization and returns the result of the shelter deletion operation.
    /// </summary>
    /// <remarks>The mapped endpoint responds to DELETE requests at '/api/shelters/{id:guid}', where 'id' is
    /// the GUID of the shelter to delete. The endpoint returns a 200 OK response with a <see
    /// cref="DeleteShelterResponseDto"/> if successful, a 401 Unauthorized if the caller lacks permissions, a 404 Not
    /// Found if the shelter does not exist, and a 500 Internal Server Error for unexpected failures.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the DELETE shelter endpoint will be mapped.</param>
    public static void MapDeleteShelterEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/shelters/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DeleteShelterEndpoint");

            var command = new DeleteShelterCommand(id);
            var response = await mediator.Send(command);

            logger.LogInformation("Shelter {ShelterId} deleted", id);

            return Results.Ok(response);
        })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("DeleteShelter")
        .WithTags("Shelters")
        .Produces<DeleteShelterResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
