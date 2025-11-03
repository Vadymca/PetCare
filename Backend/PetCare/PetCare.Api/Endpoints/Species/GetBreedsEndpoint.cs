namespace PetCare.Api.Endpoints.Species;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Species.GetBreeds;

/// <summary>
/// Provides extension methods for mapping the breeds retrieval endpoint to a web application.
/// </summary>
/// <remarks>This class contains methods for configuring HTTP GET endpoints that return breed information for a
/// specified species. The endpoint is mapped to '/api/species/{id}/breeds' and is intended to be used within ASP.NET
/// Core minimal API applications. The endpoint returns a 200 OK response with breed data if the species exists, or a
/// 404 Not Found response if the species is not found.</remarks>
public static class GetBreedsEndpoint
{
    /// <summary>
    /// Maps the HTTP GET endpoint for retrieving breeds associated with a specified species to the application's
    /// request pipeline.
    /// </summary>
    /// <remarks>The mapped endpoint responds to GET requests at '/api/species/{id}/breeds', returning a list
    /// of breeds for the species identified by the specified ID. The response is 200 OK with breed data if the species
    /// exists, or 404 Not Found if the species is not found.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoint will be mapped.</param>
    public static void MapGetBreedsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/species/{id}/breeds", async (
            Guid id,
            IMediator mediator) =>
        {
            var command = new GetBreedsCommand(id);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("GetSpeciesBreeds")
        .WithTags("Species")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<GetBreedsResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
