namespace PetCare.Api.Endpoints.Breeds;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Breeds.GetBreedById;

/// <summary>
/// Configures the endpoint for retrieving detailed information about a specific breed.
/// </summary>
public static class GetBreedByIdEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for retrieving a breed by its ID to the specified web application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoint will be added.</param>
    public static void MapGetBreedByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/breeds/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            try
            {
                var command = new GetBreedByIdCommand(id);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound(new { error = $"Порода з Id '{id}' не знайдена." });
            }
        })
        .WithName("GetBreedById")
        .WithTags("Breeds")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<BreedWithSpecieDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
