namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.AdoptionApplications.Approve;

/// <summary>
/// Endpoint for approving an adoption application.
/// </summary>
public static class ApproveAdoptionApplicationEndpoint
{
    /// <summary>
    /// Maps POST /api/adoption-applications/{id}/approve.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapApproveAdoptionApplicationEndpoint(this WebApplication app)
    {
        app.MapPost("/api/adoption-applications/{id:guid}/approve", async (
            Guid id,
            [FromBody] ApproveAdoptionApplicationCommand command,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ApproveAdoptionApplicationEndpoint");

            // Ensure command has the correct ID from route
            var commandWithId = command with { Id = id };

            await mediator.Send(commandWithId);

            logger.LogInformation("Adoption application {ApplicationId} approved by admin {AdminId}", id, command.AdminId);

            return Results.NoContent();
        })
        .WithName("ApproveAdoptionApplication")
        .WithTags("AdoptionApplications")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
