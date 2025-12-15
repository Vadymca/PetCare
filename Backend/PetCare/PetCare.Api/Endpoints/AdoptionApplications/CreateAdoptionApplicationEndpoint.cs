namespace PetCare.Api.Endpoints.AdoptionApplications;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Features.AdoptionApplications.Create;

/// <summary>
/// Endpoint for creating a new adoption application.
/// </summary>
public static class CreateAdoptionApplicationEndpoint
{
    /// <summary>
    /// Maps POST /api/adoption-applications.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void MapCreateAdoptionApplicationEndpoint(this WebApplication app)
    {
        app.MapPost("/api/adoption-applications", async (
            HttpContext httpContext,
            [FromBody] CreateAdoptionApplicationRequest request,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CreateAdoptionApplicationEndpoint");

            // Extract userId from JWT token
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Unauthorized access attempt to create adoption application");
                return Results.Unauthorized();
            }

            // Create command with userId from token
            var command = new CreateAdoptionApplicationCommand(
                UserId: userId,
                AnimalId: request.AnimalId,
                Comment: request.Comment);

            var createdApplication = await mediator.Send(command);

            logger.LogInformation(
                "Adoption application created with ID {ApplicationId} by user {UserId}",
                createdApplication.Id,
                createdApplication.UserId);

            return Results.Created($"/api/adoption-applications/{createdApplication.Id}", createdApplication);
        })
        .WithName("CreateAdoptionApplication")
        .WithTags("AdoptionApplications")
        .RequireAuthorization()
        .RequireRateLimiting("GlobalPolicy")
        .Produces<AdoptionApplicationDetailsDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    /// Request body for creating a new adoption application (does not include userId).
    /// </summary>
    public sealed record CreateAdoptionApplicationRequest(
        Guid AnimalId,
        string? Comment);
}
