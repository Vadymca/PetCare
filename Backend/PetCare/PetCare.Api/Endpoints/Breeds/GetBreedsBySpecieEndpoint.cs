namespace PetCare.Api.Endpoints.Breeds;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Breeds.GetBreedsBySpecie;

/// <summary>
/// Provides extension methods for configuring the endpoint that retrieves all breeds associated with a specified
/// species.
/// </summary>
/// <remarks>This class contains static methods for mapping the GET endpoint
/// '/api/breeds/by-species/{speciesId:guid}' to the application's request pipeline. The endpoint returns a list of
/// breeds for the given species identifier. If the specified species does not exist, the endpoint responds with a 404
/// Not Found status.</remarks>
public static class GetBreedsBySpecieEndpoint
{
    /// <summary>
    /// Maps an HTTP GET endpoint that retrieves all breeds associated with a specified species.
    /// </summary>
    /// <remarks>The mapped endpoint responds to GET requests at '/api/breeds/by-species/{speciesId:guid}'.
    /// Returns a 200 OK response with breed data if the species exists, or a 404 Not Found response if the species is
    /// not found.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoint will be added.</param>
    public static void MapGetBreedsBySpecieEndpoint(this WebApplication app)
    {
        app.MapGet("/api/breeds/by-species/{speciesId:guid}", async (
            Guid speciesId,
            IMediator mediator) =>
        {
            try
            {
                var command = new GetBreedsBySpecieCommand(speciesId);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound(new { error = $"Вид з Id '{speciesId}' не знайдено." });
            }
        })
        .WithName("GetBreedsBySpecie")
        .WithTags("Breeds")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<GetBreedsResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
