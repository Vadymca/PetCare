namespace PetCare.Api.Endpoints.Species;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Species.CreateSpecie;

/// <summary>
/// Provides extension methods for configuring the endpoint that handles creation of new species via the API.
/// </summary>
/// <remarks>This class contains extension methods for registering the species creation endpoint with a <see
/// cref="WebApplication"/> instance. The endpoint accepts POST requests to "/api/species" and returns a <see
/// cref="SpecieListDto"/> object upon successful creation. Use these methods during application startup to enable
/// species creation functionality in the API.</remarks>
public static class CreateSpecieEndpoint
{
    /// <summary>
    /// Maps the endpoint for creating a new species to the specified web application. The endpoint accepts a POST
    /// request to '/api/species' with the species creation data in the request body.
    /// </summary>
    /// <remarks>The mapped endpoint expects a JSON payload representing the species to create and returns a
    /// 201 Created response with the created species data on success, or a 400 Bad Request if the input is invalid. The
    /// endpoint is named 'CreateSpecie' and tagged with 'Species' for API documentation purposes.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the create species endpoint will be added.</param>
    public static void MapCreateSpecieEndpoint(this WebApplication app)
    {
        app.MapPost("/api/species", async (
            [FromBody] CreateSpecieCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/species/{result.Id}", result);
        })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("CreateSpecie")
        .WithTags("Species")
        .Produces<SpecieListDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
