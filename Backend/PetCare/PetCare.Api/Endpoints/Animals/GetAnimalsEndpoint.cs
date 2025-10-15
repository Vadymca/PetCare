namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.GetAnimals;
using PetCare.Domain.Enums;

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
        app.MapGet("/api/animals", async (
            IMediator mediator,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery(Name = "genders")] string[]? gendersStr = null,
            [FromQuery(Name = "sizes")] string[]? sizesStr = null,
            [FromQuery(Name = "statuses")] string[]? statusesStr = null,
            [FromQuery] int? minAge = null,
            [FromQuery] int? maxAge = null,
            [FromQuery] bool? isSterilized = null,
            [FromQuery] Guid? shelterId = null,
            [FromQuery] Guid? specieId = null,
            [FromQuery] Guid? breedId = null,
            [FromQuery] string? search = null) =>
        {
            // Конвертуємо рядки у enum, регістронезалежно
            AnimalGender[]? genders = gendersStr?.Select(s =>
                Enum.Parse<AnimalGender>(s, ignoreCase: true)).ToArray();

            AnimalSize[]? sizes = sizesStr?.Select(s =>
                Enum.Parse<AnimalSize>(s, ignoreCase: true)).ToArray();

            AnimalStatus[]? statuses = statusesStr?.Select(s =>
                Enum.Parse<AnimalStatus>(s, ignoreCase: true)).ToArray();

            var command = new GetAnimalsCommand(
                Page: page,
                PageSize: pageSize,
                Genders: genders,
                Sizes: sizes,
                Statuses: statuses,
                MinAge: minAge,
                MaxAge: maxAge,
                IsSterilized: isSterilized,
                ShelterId: shelterId,
                SpecieId: specieId,
                BreedId: breedId,
                Search: search);

            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("GetAnimals")
        .WithTags("Animals")
        .Produces<GetAnimalsResponseDto>(StatusCodes.Status200OK);
    }
}
