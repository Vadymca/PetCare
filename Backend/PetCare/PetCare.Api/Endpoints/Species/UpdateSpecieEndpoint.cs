namespace PetCare.Api.Endpoints.Species;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Species.UpdateSpecie;

/// <summary>
/// Configures the endpoint for updating an existing species.
/// </summary>
public static class UpdateSpecieEndpoint
{
    /// <summary>
    /// Maps the PUT endpoint for updating a species to the web application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapUpdateSpecieEndpoint(this WebApplication app)
    {
        app.MapPut("/api/species/{id}", async (
            Guid id,
            [FromBody] UpdateSpecieBody body,
            IMediator mediator) =>
        {
            var command = new UpdateSpecieCommand(id, body.NewName);

            var updatedSpecie = await mediator.Send(command);
            return Results.Ok(updatedSpecie);
        })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("UpdateSpecie")
        .WithTags("Species")
        .Produces<SpecieDetailDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
