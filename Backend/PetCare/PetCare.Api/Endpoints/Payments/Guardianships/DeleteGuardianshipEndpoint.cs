namespace PetCare.Api.Endpoints.Payments.Guardianships;

using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.Guardianships.DeleteGuardianship;

/// <summary>
/// Endpoint for deleting a guardianship.
/// </summary>
public static class DeleteGuardianshipEndpoint
{
    /// <summary>
    /// Maps the DELETE /api/guardianships/{id} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapDeleteGuardianshipEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/guardianships/{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            IMediator mediator,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("DeleteGuardianshipEndpoint");

            // Validate user
            var userIdClaim = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized attempt to delete guardianship {Id}", id);
                return Results.Unauthorized();
            }

            var command = new DeleteGuardianshipCommand(id, userId);
            var response = await mediator.Send(command, cancellationToken);

            logger.LogInformation("Guardianship {GuardianshipId} deleted by user {UserId}", id, userId);

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("DeleteGuardianship")
        .WithTags("Payments.Guardianships")
        .Produces<GuardianshipDeletedDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireRateLimiting("GlobalPolicy");
    }
}
