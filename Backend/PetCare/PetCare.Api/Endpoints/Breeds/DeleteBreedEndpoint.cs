namespace PetCare.Api.Endpoints.Breeds;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Breeds.DeleteBreed;

/// <summary>
/// Endpoint for deleting a breed by its ID.
/// </summary>
public static class DeleteBreedEndpoint
{
    /// <summary>
    /// Maps the DELETE /api/breeds/{id} endpoint to the specified web application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapDeleteBreedEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/breeds/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DeleteBreedEndpoint");

            var command = new DeleteBreedCommand(id);
            var response = await mediator.Send(command);

            logger.LogInformation("Breed {BreedId} deleted", id);

            return Results.Ok(response);
        })
        .RequireAuthorization("AdminOnly") // Only admins can delete
        .RequireRateLimiting("GlobalPolicy")
        .WithName("DeleteBreed")
        .WithTags("Breeds")
        .Produces<DeleteBreedResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
