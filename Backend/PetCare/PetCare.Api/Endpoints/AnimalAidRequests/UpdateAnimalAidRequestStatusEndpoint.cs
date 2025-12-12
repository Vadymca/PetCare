namespace PetCare.Api.Endpoints.AnimalAidRequests;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Features.AnimalAidRequests.UpdateStatus;

/// <summary>
/// Endpoint for updating the status of an existing animal aid request.
/// </summary>
public static class UpdateAnimalAidRequestStatusEndpoint
{
    /// <summary>
    /// Maps PATCH /api/animal-aid-requests/{id}/status.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapUpdateAnimalAidRequestStatusEndpoint(this WebApplication app)
    {
        app.MapPatch("/api/animal-aid-requests/{id:guid}/status", async (
            Guid id,
            [FromBody] UpdateAnimalAidRequestStatusCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UpdateAnimalAidRequestStatusEndpoint");

            // Ensure command has the correct ID from route
            var commandWithId = command with { Id = id };

            var updatedRequest = await mediator.Send(commandWithId);

            logger.LogInformation("Animal aid request {RequestId} status updated to {Status}", id, updatedRequest.Status);

            return Results.Ok(updatedRequest);
        })
        .WithName("UpdateAnimalAidRequestStatus")
        .WithTags("AnimalAidRequests")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<AnimalAidRequestDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
