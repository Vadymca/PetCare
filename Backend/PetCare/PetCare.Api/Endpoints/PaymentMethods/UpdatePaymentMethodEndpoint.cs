namespace PetCare.Api.Endpoints.PaymentMethods;

using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.PaymentMethods.UpdatePaymentMethod;

/// <summary>
/// Provides extension methods for configuring the endpoint that updates an existing payment method via an HTTP PUT
/// request.
/// </summary>
/// <remarks>The update endpoint is mapped to '/api/payment-methods/{id:guid}' and expects a GUID identifier in
/// the route and an update command in the request body. The endpoint enforces rate limiting using the 'GlobalPolicy'
/// and returns a 200 OK response with the updated payment method data or a 400 Bad Request if the route ID does not
/// match the command ID. This class is intended to be used during application startup to register the update endpoint
/// with a WebApplication instance.</remarks>
public static class UpdatePaymentMethodEndpoint
{
    /// <summary>
    /// Maps the endpoint for updating an existing payment method to the specified web application.
    /// </summary>
    /// <remarks>The mapped endpoint handles HTTP PUT requests to '/api/payment-methods/{id:guid}' and
    /// requires rate limiting under the 'GlobalPolicy'. The endpoint returns a 200 OK response with the updated payment
    /// method data if successful, or a 400 Bad Request response if the ID in the URL does not match the ID in the
    /// request body.</remarks>
    /// <param name="app">The web application to which the update payment method endpoint will be added.</param>
    public static void MapUpdatePaymentMethodEndpoint(this WebApplication app)
    {
        app.MapPut("/api/payment-methods/{id:guid}", async (Guid id, UpdatePaymentMethodCommand command, IMediator mediator) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID в URL і тілі запиту не співпадають.");
            }

            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdatePaymentMethod")
        .WithTags("Payment Methods")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<PaymentMethodDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
