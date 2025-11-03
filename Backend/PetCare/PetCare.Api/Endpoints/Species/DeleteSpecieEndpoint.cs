namespace PetCare.Api.Endpoints.Species;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Species.DeleteSpecie;

/// <summary>
/// Endpoint for deleting a species by its ID.
/// </summary>
public static class DeleteSpecieEndpoint
{
    /// <summary>
    /// Maps the DELETE /api/species/{id} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapDeleteSpecieEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/species/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DeleteSpecieEndpoint");

            var command = new DeleteSpecieCommand(id);
            var response = await mediator.Send(command);

            logger.LogInformation("Species {SpecieId} deleted", id);

            return Results.Ok(response);
        })
        .RequireAuthorization("AdminOnly") // Only admins can delete species
        .RequireRateLimiting("GlobalPolicy")
        .WithName("DeleteSpecie")
        .WithTags("Species")
        .Produces<DeleteSpecieResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
