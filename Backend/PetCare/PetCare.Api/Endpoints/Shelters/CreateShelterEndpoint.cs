namespace PetCare.Api.Endpoints.Shelters;

using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.CreateShelter;

/// <summary>
/// Endpoint for creating a new shelter (Admin only).
/// </summary>
public static class CreateShelterEndpoint
{
    /// <summary>
    /// Maps the POST /api/shelters endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapCreateShelterEndpoint(this WebApplication app)
    {
        app.MapPost("/api/shelters", async (
            CreateShelterBody body,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CreateShelterEndpoint");

            var command = new CreateShelterCommand(
                body.Name,
                body.Address,
                body.Latitude,
                body.Longitude,
                body.ContactPhone,
                body.ContactEmail,
                body.Description,
                body.Capacity,
                body.Photos,
                body.VirtualTourUrl,
                body.WorkingHours,
                body.SocialMedia,
                body.ManagerId);

            var addedShelter = await mediator.Send(command);

            logger.LogInformation("Shelter {ShelterId} created by admin", addedShelter.Id);

            return Results.Created($"/api/shelters/{addedShelter.Id}", addedShelter);
        })
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .WithName("CreateShelter")
        .WithTags("Shelters")
        .Produces<ShelterDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden);
    }
}
