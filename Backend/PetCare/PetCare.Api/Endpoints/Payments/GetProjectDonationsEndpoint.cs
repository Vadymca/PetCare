namespace PetCare.Api.Endpoints.Payments;

using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.GetProjectDonations;

/// <summary>
/// Endpoint for retrieving donations related to a specific project.
/// </summary>
public static class GetProjectDonationsEndpoint
{
    /// <summary>
    /// Maps the GET /api/payments/project/{projectId} endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetProjectDonationsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/payments/project/{projectId:guid}", async (
            Guid projectId,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetProjectDonationsEndpoint");

            var result = await mediator.Send(new GetProjectDonationsCommand(projectId));

            logger.LogInformation(
                "Retrieved {Count} donations for project {ProjectId}.",
                result.Count,
                projectId);

            return Results.Ok(result);
        })
        .WithName("GetProjectDonations")
        .WithTags("Payments")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<IReadOnlyList<DonationListDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
