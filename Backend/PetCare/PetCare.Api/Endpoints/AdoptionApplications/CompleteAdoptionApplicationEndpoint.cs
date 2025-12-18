namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.AdoptionApplications.CompleteAdoption;

/// <summary>
/// Endpoint for completing the adoption process.
/// </summary>
public static class CompleteAdoptionApplicationEndpoint
{
    /// <summary>
    /// Maps POST /api/adoption-applications/{id}/complete.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapCompleteAdoptionApplicationEndpoint(this WebApplication app)
    {
        app.MapPost("/api/adoption-applications/{id:guid}/complete", async (
            Guid id,
            [FromBody] CompleteAdoptionRequest request,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CompleteAdoptionApplicationEndpoint");

            // Ensure command has the correct ApplicationId from route
            var command = new CompleteAdoptionCommand(
                ApplicationId: id,
                IsAdopted: request.IsAdopted);

            await mediator.Send(command);

            logger.LogInformation(
                "Adoption application {ApplicationId} completed. IsAdopted: {IsAdopted}",
                id,
                command.IsAdopted);

            return Results.NoContent();
        })
        .WithName("CompleteAdoptionApplication")
        .WithTags("AdoptionApplications")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Request body for completing an adoption.
    /// </summary>
    public sealed record CompleteAdoptionRequest(
        bool IsAdopted);
}
