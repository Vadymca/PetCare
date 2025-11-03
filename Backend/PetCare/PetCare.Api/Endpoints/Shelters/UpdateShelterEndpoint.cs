namespace PetCare.Api.Endpoints.Shelters;

using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.UpdateShelter;

/// <summary>
/// Provides extension methods for configuring the endpoint that updates an existing shelter resource in the API.
/// </summary>
/// <remarks>This class contains methods for mapping the HTTP PUT endpoint used to update shelter information. The
/// endpoint requires authorization with the "CanManageShelters" policy and is tagged as "Shelters" for API
/// documentation purposes.</remarks>
public static class UpdateShelterEndpoint
{
    /// <summary>
    /// Maps the HTTP PUT endpoint for updating an existing shelter to the specified web application. The endpoint
    /// allows authorized users to update shelter details by ID.
    /// </summary>
    /// <remarks>The mapped endpoint requires the "CanManageShelters" authorization policy. It responds with
    /// status code 200 and the updated shelter data if successful, 400 for invalid input, 403 if the user is not
    /// authorized, and 404 if the shelter is not found.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the update shelter endpoint will be mapped.</param>
    public static void MapUpdateShelterEndpoint(this WebApplication app)
    {
        app.MapPut("/api/shelters/{id:guid}", async (
            Guid id,
            UpdateShelterBody body,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
                {
                    var logger = loggerFactory.CreateLogger("UpdateShelterEndpoint");

                    var command = new UpdateShelterCommand(
                        Id: id,
                        Name: body.Name,
                        Address: body.Address,
                        Latitude: body.Latitude,
                        Longitude: body.Longitude,
                        ContactPhone: body.ContactPhone,
                        ContactEmail: body.ContactEmail,
                        Description: body.Description,
                        Capacity: body.Capacity,
                        Photos: body.Photos,
                        VirtualTourUrl: body.VirtualTourUrl,
                        WorkingHours: body.WorkingHours,
                        SocialMedia: body.SocialMedia);

                    var updatedShelter = await mediator.Send(command);

                    logger.LogInformation("Shelter {ShelterId} updated", id);

                    return Results.Ok(updatedShelter);
                })
        .RequireAuthorization("CanManageShelter")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("UpdateShelter")
        .WithTags("Shelters")
        .Produces<ShelterDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound);
    }
}
