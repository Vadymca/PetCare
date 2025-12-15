namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Features.AdoptionApplications.Update;

/// <summary>
/// Endpoint for updating an existing adoption application.
/// </summary>
public static class UpdateAdoptionApplicationEndpoint
{
    /// <summary>
    /// Maps PUT /api/adoption-applications/{id}.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    public static void MapUpdateAdoptionApplicationEndpoint(this WebApplication app)
    {
        app.MapPut("/api/adoption-applications/{id:guid}", async (
            Guid id,
            [FromBody] UpdateAdoptionApplicationCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UpdateAdoptionApplicationEndpoint");

            // Ensure the command has the correct ID from route
            var commandWithId = command with { Id = id };

            var updatedApplication = await mediator.Send(commandWithId);

            logger.LogInformation("Adoption application {ApplicationId} updated.", id);

            return Results.Ok(updatedApplication);
        })
        .WithName("UpdateAdoptionApplication")
        .WithTags("AdoptionApplications")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<AdoptionApplicationDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
