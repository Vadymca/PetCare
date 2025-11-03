namespace PetCare.Api.Endpoints.Species;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Species.GetSpecies;

/// <summary>
/// Provides extension methods for configuring the species retrieval API endpoint in an ASP.NET Core application.
/// </summary>
/// <remarks>This class contains static methods for mapping the GET endpoint that returns species data. The
/// endpoint is registered at '/api/species' and is intended to be used with minimal API routing. The mapped endpoint
/// responds with a 200 OK status and a payload of type GetSpeciesResponseDto. This class is not intended to be
/// instantiated.</remarks>
public static class GetSpeciesEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for retrieving species data to the specified web application.
    /// </summary>
    /// <remarks>The mapped endpoint responds to GET requests at '/api/species' and returns species
    /// information in a <see cref="GetSpeciesResponseDto"/> object with a 200 OK status. The endpoint is named
    /// 'GetSpecies' and tagged with 'Species' for API documentation purposes.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the species GET endpoint will be added.</param>
    public static void MapGetSpeciesEndpoint(this WebApplication app)
    {
        app.MapGet("/api/species", async (IMediator mediator) =>
        {
            var command = new GetSpeciesCommand();
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("GetSpecies")
        .WithTags("Species")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<GetSpeciesResponseDto>(StatusCodes.Status200OK);
    }
}
