namespace PetCare.Api.Endpoints.PaymentMethods;

using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.PaymentMethods.GetAllPaymentMethods;

/// <summary>
/// Provides extension methods for configuring the endpoint that retrieves all available payment methods via the API.
/// </summary>
/// <remarks>This static class contains methods for mapping the GET endpoint '/api/payment-methods' to the
/// application's request pipeline. The endpoint returns a list of payment methods and applies global rate limiting. Use
/// these methods to register the endpoint during application startup.</remarks>
public static class GetAllPaymentMethodsEndpoint
{
    /// <summary>
    /// Maps the endpoint for retrieving all available payment methods to the specified web application.
    /// </summary>
    /// <remarks>The mapped endpoint responds to HTTP GET requests at '/api/payment-methods' and returns a
    /// list of payment methods. The endpoint is tagged as 'Payment Methods', requires the 'GlobalPolicy' rate limiting
    /// policy, and produces a 200 OK response with a read-only list of <see cref="PaymentMethodDto"/>
    /// objects.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoint will be mapped.</param>
    public static void MapGetAllPaymentMethodsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/payment-methods", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllPaymentMethodsCommand());
            return Results.Ok(result);
        })
        .WithName("GetAllPaymentMethods")
        .WithTags("Payment Methods")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<IReadOnlyList<PaymentMethodDto>>(StatusCodes.Status200OK);
    }
}
