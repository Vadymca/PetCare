namespace PetCare.Api.Endpoints.AnimalAidRequests;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Features.AnimalAidRequests.Create;

/// <summary>
/// Endpoint for creating a new animal aid request.
/// </summary>
public static class CreateAnimalAidRequestEndpoint
{
    /// <summary>
    /// Maps POST /api/animal-aid-requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapCreateAnimalAidRequestEndpoint(this WebApplication app)
    {
        app.MapPost("/api/animal-aid-requests", async (
            [FromBody] CreateAnimalAidRequestCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CreateAnimalAidRequestEndpoint");

            var createdRequest = await mediator.Send(command);

            logger.LogInformation("Animal aid request created with ID {RequestId}", createdRequest.Id);

            return Results.Created($"/api/animal-aid-requests/{createdRequest.Id}", createdRequest);
        })
        .WithName("CreateAnimalAidRequest")
        .WithTags("AnimalAidRequests")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<AnimalAidRequestDetailsDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
