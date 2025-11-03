namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.DeleteAnimal;

/// <summary>
/// Endpoint for deleting an animal by its ID.
/// </summary>
public static class DeleteAnimalEndpoint
{
    /// <summary>
    /// Maps the DELETE /api/animals/{id} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapDeleteAnimalEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/animals/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DeleteAnimalEndpoint");

            var command = new DeleteAnimalCommand(id);
            var response = await mediator.Send(command);

            logger.LogInformation("Animal {AnimalId} deleted", id);

            return Results.Ok(response);
        })
        .RequireAuthorization("AdminOnly") // Admin
        .WithName("DeleteAnimal")
        .WithTags("Animals")
        .Produces<DeleteAnimalResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireRateLimiting("GlobalPolicy");
    }
}
