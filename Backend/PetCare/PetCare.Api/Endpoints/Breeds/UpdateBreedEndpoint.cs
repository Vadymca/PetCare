namespace PetCare.Api.Endpoints.Breeds;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Features.Breeds.UpdateBreed;

/// <summary>
/// Provides extension methods for configuring the endpoint that updates an existing breed resource in the API.
/// </summary>
/// <remarks>This class contains methods for mapping the HTTP PUT endpoint used to update breed information. The
/// endpoint requires authorization with the "AdminOnly" policy and is tagged as "Breeds". It returns a 200 OK response
/// with the updated breed data, a 400 Bad Request if the route and body IDs do not match, or a 404 Not Found if the
/// breed does not exist.</remarks>
public static class UpdateBreedEndpoint
{
    /// <summary>
    /// Maps the HTTP PUT endpoint for updating an existing breed in the application.
    /// </summary>
    /// <remarks>The endpoint requires 'AdminOnly' authorization and responds with status codes 200 (OK), 400
    /// (Bad Request), or 404 (Not Found). The route expects a breed identifier in the URL and an update command in the
    /// request body. If the identifier in the URL does not match the one in the request body, a bad request response is
    /// returned.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the update breed endpoint will be added.</param>
    public static void MapUpdateBreedEndpoint(this WebApplication app)
    {
        app.MapPut("/api/breeds/{id:guid}", async (
            Guid id,
            [FromBody] UpdateBreedCommand command,
            IMediator mediator) =>
                    {
                        if (id != command.Id)
                        {
                            return Results.BadRequest("Id у шляху не співпадає з Id у тілі запиту.");
                        }

                        var updatedBreed = await mediator.Send(command);
                        return Results.Ok(updatedBreed);
                    })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("UpdateBreed")
        .WithTags("Breeds")
        .Produces<BreedWithSpecieDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
