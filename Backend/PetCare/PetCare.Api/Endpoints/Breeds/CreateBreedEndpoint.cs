namespace PetCare.Api.Endpoints.Breeds;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Breeds.CreateBreed;

/// <summary>
/// Provides an extension method to map the endpoint for creating a new breed in the API.
/// </summary>
/// <remarks>This class is intended to be used during application startup to register the POST endpoint for
/// creating breeds. The mapped endpoint requires 'AdminOnly' authorization and returns a 201 Created response with the
/// created breed, or appropriate error responses for invalid input or unauthorized access.</remarks>
public static class CreateBreedEndpoint
{
    /// <summary>
    /// Maps the endpoint for creating a new breed to the specified web application. The endpoint handles HTTP POST
    /// requests to '/api/breeds' and requires 'AdminOnly' authorization.
    /// </summary>
    /// <remarks>The mapped endpoint expects a <see cref="CreateBreedCommand"/> in the request body and
    /// returns a <see cref="BreedWithSpecieDto"/> with a 201 Created status on success. It returns 400 Bad Request for
    /// invalid input and 401 Unauthorized if the caller lacks the required authorization.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the create breed endpoint will be mapped.</param>
    public static void MapCreateBreedEndpoint(this WebApplication app)
    {
        app.MapPost("/api/breeds", async (
            [FromBody] CreateBreedCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/breeds/{result.Id}", result);
        })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("CreateBreed")
        .WithTags("Breeds")
        .Produces<BreedWithSpecieDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
