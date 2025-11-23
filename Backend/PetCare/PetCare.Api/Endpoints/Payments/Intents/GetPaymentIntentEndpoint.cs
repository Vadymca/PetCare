namespace PetCare.Api.Endpoints.Payments.Intents;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.Intents;

/// <summary>
/// Endpoint for retrieving detailed information about a payment intent by its external order identifier.
/// </summary>
public static class GetPaymentIntentEndpoint
{
    /// <summary>
    /// Maps GET /api/payments/intents/{orderId} to retrieve a payment intent.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    public static void MapGetPaymentIntentEndpoint(this WebApplication app)
    {
        app.MapGet("/api/payments/intents/{orderId}", async (
            [FromRoute] string orderId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return Results.BadRequest("OrderId cannot be empty.");
            }

            var command = new GetPaymentIntentByOrderIdCommand(orderId);
            var response = await mediator.Send(command, cancellationToken);

            return response is null
                ? Results.NotFound()
                : Results.Ok(response);
        })
        .WithName("GetPaymentIntentByOrderId")
        .WithTags("Payments.Intents")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<PaymentIntentDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
