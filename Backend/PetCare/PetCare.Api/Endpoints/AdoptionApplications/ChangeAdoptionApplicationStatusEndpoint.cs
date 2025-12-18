namespace PetCare.Api.Endpoints.AdoptionApplications;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.AdoptionApplications.ChangeStatus;
using PetCare.Domain.Enums;

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
            HttpContext httpContext,
            [FromBody] ChangeAdoptionApplicationStatusRequest request,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ChangeAdoptionApplicationStatusEndpoint");

            var adminIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(adminIdClaim, out var adminId))
            {
                logger.LogWarning(
                    "Unauthorized attempt to change adoption application status. ApplicationId={ApplicationId}",
                    id);

                return Results.Unauthorized();
            }

            var command = new ChangeAdoptionApplicationStatusCommand(
                Id: id,
                Status: request.Status,
                AdminId: adminId,
                RejectionReason: request.RejectionReason,
                CuratorName: request.CuratorName,
                CuratorPhone: request.CuratorPhone,
                MeetingDate: request.MeetingDate);

            await mediator.Send(command);

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

    /// <summary>
    /// Request body for changing adoption application status.
    /// </summary>
    public sealed record ChangeAdoptionApplicationStatusRequest(
        AdoptionStatus Status,
        string? RejectionReason,
        string? CuratorName = null,
        string? CuratorPhone = null,
        DateTime? MeetingDate = null);
}
