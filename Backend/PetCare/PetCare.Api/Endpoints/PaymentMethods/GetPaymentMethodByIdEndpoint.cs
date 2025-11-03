namespace PetCare.Api.Endpoints.PaymentMethods;

using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.PaymentMethods.GetPaymentMethodById;

/// <summary>
/// Provides extension methods for configuring the endpoint that retrieves a payment method by its unique identifier.
/// </summary>
/// <remarks>This class contains methods for mapping the GET endpoint '/api/payment-methods/{id:guid}' to the
/// application's request pipeline. The endpoint returns a payment method if found, or a 404 Not Found response if the
/// specified identifier does not correspond to an existing payment method. Use these methods to register the endpoint
/// during application startup.</remarks>
public static class GetPaymentMethodByIdEndpoint
{
    /// <summary>
    /// Maps an HTTP GET endpoint for retrieving a payment method by its unique identifier.
    /// </summary>
    /// <remarks>The endpoint responds to GET requests at '/api/payment-methods/{id:guid}', returning a <see
    /// cref="PaymentMethodDto"/> if found, or a 404 status code if not. The endpoint is tagged as 'Payment Methods',
    /// named 'GetPaymentMethodById', and enforces the 'GlobalPolicy' rate limiting policy.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoint will be added.</param>
    public static void MapGetPaymentMethodByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/api/payment-methods/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPaymentMethodByIdCommand(id));
            return Results.Ok(result);
        })
        .WithName("GetPaymentMethodById")
        .WithTags("Payment Methods")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<PaymentMethodDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
