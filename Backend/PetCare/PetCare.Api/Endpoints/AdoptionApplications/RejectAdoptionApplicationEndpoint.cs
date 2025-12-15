namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.AdoptionApplications.Reject;

/// <summary>
/// Endpoint for rejecting an adoption application.
/// </summary>
public static class RejectAdoptionApplicationEndpoint
{
    /// <summary>
    /// Maps POST /api/adoption-applications/{id}/reject.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapRejectAdoptionApplicationEndpoint(this WebApplication app)
    {
        app.MapPost("/api/adoption-applications/{id:guid}/reject", async (
            Guid id,
            [FromBody] RejectAdoptionApplicationCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RejectAdoptionApplicationEndpoint");

            // Ensure command has the correct ID from route
            var commandWithId = command with { Id = id };

            await mediator.Send(commandWithId);

            logger.LogInformation("Adoption application {ApplicationId} rejected. Reason: {Reason}", id, command.Reason);

            return Results.NoContent();
        })
        .WithName("RejectAdoptionApplication")
        .WithTags("AdoptionApplications")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
