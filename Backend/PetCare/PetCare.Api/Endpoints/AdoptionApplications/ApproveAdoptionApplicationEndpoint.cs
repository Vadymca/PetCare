namespace PetCare.Api.Endpoints.AdoptionApplications;

using System.Security.Claims;
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
            HttpContext httpContext,
            [FromBody] ApproveAdoptionApplicationRequest request,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ApproveAdoptionApplicationEndpoint");

            var adminIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(adminIdClaim, out var adminId))
            {
                logger.LogWarning(
                    "Unauthorized attempt to approve adoption application {ApplicationId}",
                    id);

                return Results.Unauthorized();
            }

            var command = new ApproveAdoptionApplicationCommand(
                Id: id,
                AdminId: adminId,
                CuratorName: request.CuratorName,
                CuratorPhone: request.CuratorPhone,
                MeetingDate: request.MeetingDate);

            await mediator.Send(command);

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

    /// <summary>
    /// Request body for approving an adoption application.
    /// </summary>
    public sealed record ApproveAdoptionApplicationRequest(
        string? CuratorName,
        string? CuratorPhone,
        DateTime? MeetingDate);
}
