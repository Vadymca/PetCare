namespace PetCare.Api.Endpoints.Species;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Species.GetSpecieById;

/// <summary>
/// Provides extension methods for configuring the endpoint that retrieves a species by its unique identifier.
/// </summary>
/// <remarks>This class contains methods for mapping the GET endpoint '/api/species/{id}' to retrieve detailed
/// information about a species. The endpoint is registered with the 'Species' tag and returns a 200 OK response with a
/// <see cref="SpecieDetailDto"/> object when successful.</remarks>
public static class GetSpecieByIdEndpoint
{
    /// <summary>
    /// Maps an HTTP GET endpoint to retrieve detailed information about a species by its unique identifier.
    /// </summary>
    /// <remarks>The mapped endpoint responds to GET requests at '/api/species/{id}' and returns a <see
    /// cref="SpecieDetailDto"/> with status code 200 if the species is found. The endpoint is named 'GetSpecieById' and
    /// tagged with 'Species'.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoint will be added.</param>
    public static void MapGetSpecieByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/species/{id}", async (
            Guid id,
            IMediator mediator) =>
        {
            var command = new GetSpecieByIdCommand(id);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("GetSpecieById")
        .WithTags("Species")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<SpecieDetailDto>(StatusCodes.Status200OK);
    }
}
