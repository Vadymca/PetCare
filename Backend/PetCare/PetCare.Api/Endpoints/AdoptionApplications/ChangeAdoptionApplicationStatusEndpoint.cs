namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.AdoptionApplications.ChangeStatus;

/// <summary>
/// Endpoint for changing the status of an adoption application.
/// </summary>
public static class ChangeAdoptionApplicationStatusEndpoint
{
    /// <summary>
    /// Maps PATCH /api/adoption-applications/{id}/change-status.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapChangeAdoptionApplicationStatusEndpoint(this WebApplication app)
    {
        app.MapPatch("/api/adoption-applications/{id:guid}/change-status", async (
            Guid id,
            [FromBody] ChangeAdoptionApplicationStatusCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ChangeAdoptionApplicationStatusEndpoint");

            // Ensure the command has the correct ID from route
            var commandWithId = command with { Id = id };

            await mediator.Send(commandWithId);

            logger.LogInformation(
                "Adoption application {ApplicationId} status changed to {Status}",
                id,
                command.Status);

            return Results.NoContent();
        })
        .WithName("ChangeAdoptionApplicationStatus")
        .WithTags("AdoptionApplications")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
