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
            [FromBody] UpdateAdoptionApplicationRequest request,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UpdateAdoptionApplicationEndpoint");

            var command = new UpdateAdoptionApplicationCommand(
                Id: id,
                Comment: request.Comment,
                AdminNotes: request.AdminNotes,
                CuratorName: request.CuratorName,
                CuratorPhone: request.CuratorPhone,
                MeetingDate: request.MeetingDate,
                AdoptionDate: request.AdoptionDate);

            var updatedApplication = await mediator.Send(command);

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

    /// <summary>
    /// Request body for updating an adoption application.
    /// </summary>
    public sealed record UpdateAdoptionApplicationRequest(
        string? Comment,
        string? AdminNotes,
        string? CuratorName,
        string? CuratorPhone,
        DateTime? MeetingDate,
        DateTime? AdoptionDate);
}
