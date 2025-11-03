namespace PetCare.Api.Endpoints.Payments;

using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.GetAllDonations;

/// <summary>
/// Endpoint for retrieving a list of all donations in the system.
/// </summary>
public static class GetAllDonationsEndpoint
{
    /// <summary>
    /// Maps the GET /api/payments/all endpoint.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map the endpoint on.</param>
    public static void MapGetAllDonationsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/payments/all", async (
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GetAllDonationsEndpoint");

            var result = await mediator.Send(new GetAllDonationsCommand());

            logger.LogInformation("Retrieved {Count} donations in total.", result.Count);

            return Results.Ok(result);
        })
        .WithName("GetAllDonations")
        .WithTags("Payments")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<IReadOnlyList<DonationListDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
