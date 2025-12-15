namespace PetCare.Api.Endpoints.AdoptionApplications;

using MediatR;
using PetCare.Application.Features.AdoptionApplications.Delete;

/// <summary>
/// Endpoint for deleting an existing adoption application.
/// </summary>
public static class DeleteAdoptionApplicationEndpoint
{
    /// <summary>
    /// Maps DELETE /api/adoption-applications/{id}.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> where the endpoint will be registered.</param>
    public static void MapDeleteAdoptionApplicationEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/adoption-applications/{id:guid}", async (
            Guid id,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("DeleteAdoptionApplicationEndpoint");

            await mediator.Send(new DeleteAdoptionApplicationCommand(id));

            logger.LogInformation("Adoption application {ApplicationId} deleted", id);

            return Results.NoContent();
        })
        .WithName("DeleteAdoptionApplication")
        .WithTags("AdoptionApplications")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
