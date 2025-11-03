namespace PetCare.Api.Endpoints.Payments;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.CreateGuardianship;

/// <summary>
/// Endpoint for creating new guardianships.
/// </summary>
public static class CreateGuardianshipEndpoint
{
    /// <summary>
    /// Maps the POST /api/guardianships endpoint.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void MapCreateGuardianshipEndpoint(this WebApplication app)
    {
        app.MapPost("/api/guardianships", async (
            HttpContext httpContext,
            CreateGuardianshipRequestDto request,
            IMediator mediator,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("CreateGuardianshipEndpoint");

            // Авторизація
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to create guardianship.");
                return Results.Unauthorized();
            }

            try
            {
                var result = await mediator.Send(new CreateGuardianshipCommand(userId, request.AnimalId), cancellationToken);
                logger.LogInformation("Guardianship created successfully for User {UserId}, Animal {AnimalId}", userId, request.AnimalId);
                return Results.Created($"/api/guardianships/{result.Id}", result);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "Failed to create guardianship for User {UserId}", userId);
                return Results.BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while creating guardianship for User {UserId}", userId);
                return Results.Problem($"Unexpected error: {ex.Message}");
            }
        })
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .WithName("CreateGuardianship")
        .WithTags("Payments")
        .Produces<GuardianshipCreatedDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
