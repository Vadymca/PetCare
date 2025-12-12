namespace PetCare.Api.Endpoints.AnimalAidRequests;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Features.AnimalAidRequests.Update;

/// <summary>
/// Endpoint for updating an existing animal aid request.
/// </summary>
public static class UpdateAnimalAidRequestEndpoint
{
    /// <summary>
    /// Maps PUT /api/animal-aid-requests/{id}.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapUpdateAnimalAidRequestEndpoint(this WebApplication app)
    {
        app.MapPut("/api/animal-aid-requests/{id:guid}", async (
            Guid id,
            [FromBody] UpdateAnimalAidRequestCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UpdateAnimalAidRequestEndpoint");

            // Ensure the command has the correct ID from the route
            var commandWithId = command with { Id = id };

            var updatedRequest = await mediator.Send(commandWithId);

            logger.LogInformation("Animal aid request {RequestId} updated", id);

            return Results.Ok(updatedRequest);
        })
        .WithName("UpdateAnimalAidRequest")
        .WithTags("AnimalAidRequests")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<AnimalAidRequestDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
