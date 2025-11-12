namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.GetAnimals;

/// <summary>
/// Configures the endpoint for retrieving a paginated list of animals.
/// </summary>
/// <remarks>This endpoint is mapped to the route <c>/api/animals</c> and supports filtering by shelter, breed,
/// and search terms. It requires authorization and returns a paginated response containing animal data.</remarks>
public static class GetAnimalsEndpoint
{
    /// <summary>
    /// Maps the GET /api/animals endpoint to handle requests for retrieving a paginated list of animals.
    /// </summary>
    /// <param name="app">The web application to which the endpoint is being added.</param>
    public static void MapGetAnimalsEndpoint(this WebApplication app)
    {
        app.MapPost("/api/animals/filter", async (
            IMediator mediator,
            [FromBody] GetAnimalsCommand command) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("GetAnimals")
        .WithTags("Animals")
        .Produces<GetAnimalsResponseDto>(StatusCodes.Status200OK)
        .Accepts<GetAnimalsCommand>("application/json")
        .RequireRateLimiting("GlobalPolicy");
    }
}
