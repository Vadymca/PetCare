namespace PetCare.Api.Endpoints.PaymentMethods;

using MediatR;
using PetCare.Application.Features.PaymentMethods.DeletePaymentMethod;

/// <summary>
/// Provides extension methods for configuring the endpoint that deletes a payment method by its unique identifier.
/// </summary>
/// <remarks>The mapped endpoint responds to HTTP DELETE requests at '/api/payment-methods/{id:guid}', removing
/// the specified payment method if it exists. The endpoint returns a 204 No Content status on successful deletion, or a
/// 404 Not Found status if the payment method does not exist. Rate limiting is enforced using the 'GlobalPolicy'
/// policy. This class is intended to be used during application startup to register the delete payment method route
/// with the web application.</remarks>
public static class DeletePaymentMethodEndpoint
{
    /// <summary>
    /// Maps the endpoint for deleting a payment method by its unique identifier to the application's request pipeline.
    /// </summary>
    /// <remarks>The mapped endpoint responds to HTTP DELETE requests at '/api/payment-methods/{id:guid}'. If
    /// the specified payment method exists and is deleted successfully, the endpoint returns a 204 No Content response.
    /// If the payment method is not found, a 404 Not Found response is returned. The endpoint is tagged as 'Payment
    /// Methods', named 'DeletePaymentMethod', and is subject to the 'GlobalPolicy' rate limiting policy.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the delete payment method endpoint will be added.</param>
    public static void MapDeletePaymentMethodEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/payment-methods/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            await mediator.Send(new DeletePaymentMethodCommand(id));
            return Results.NoContent();
        })
        .WithName("DeletePaymentMethod")
        .WithTags("Payment Methods")
        .RequireRateLimiting("GlobalPolicy")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}
