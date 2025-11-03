namespace PetCare.Api.Endpoints.Breeds;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Breeds.GetAllBreeds;

/// <summary>
/// Endpoint for retrieving all breeds.
/// </summary>
public static class GetAllBreedsEndpoint
{
    /// <summary>
    /// Maps the GET /api/breeds endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapGetAllBreedsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/breeds", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBreedsCommand());
            return Results.Ok(result);
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("GetAllBreeds")
        .WithTags("Breeds")
        .Produces<GetAllBreedsResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
