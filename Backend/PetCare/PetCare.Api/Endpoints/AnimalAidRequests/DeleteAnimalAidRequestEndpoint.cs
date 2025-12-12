namespace PetCare.Api.Endpoints.AnimalAidRequests;

using MediatR;
using PetCare.Application.Features.AnimalAidRequests.Delete;

/// <summary>
/// Endpoint for deleting an existing animal aid request.
/// </summary>
public static class DeleteAnimalAidRequestEndpoint
{
    /// <summary>
    /// Maps DELETE /api/animal-aid-requests/{id}.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapDeleteAnimalAidRequestEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/animal-aid-requests/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DeleteAnimalAidRequestEndpoint");

            await mediator.Send(new DeleteAnimalAidRequestCommand(id));

            logger.LogInformation("Animal aid request {RequestId} deleted", id);

            return Results.NoContent();
        })
        .WithName("DeleteAnimalAidRequest")
        .WithTags("AnimalAidRequests")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
