namespace PetCare.Api.Endpoints.PaymentMethods;

using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.PaymentMethods.CreatePaymentMethod;

/// <summary>
/// Provides extension methods for configuring the endpoint that handles creation of payment methods in the web
/// application.
/// </summary>
/// <remarks>This static class contains methods for mapping the HTTP POST endpoint used to create new payment
/// methods. The endpoint is registered at '/api/payment-methods' and is configured with rate limiting, response types,
/// and tagging for API documentation. Use these methods to integrate payment method creation functionality into an
/// ASP.NET Core application.</remarks>
public static class CreatePaymentMethodEndpoint
{
    /// <summary>
    /// Maps the endpoint for creating a new payment method to the specified web application.
    /// </summary>
    /// <remarks>The mapped endpoint accepts POST requests at '/api/payment-methods' and requires rate
    /// limiting as defined by the 'GlobalPolicy'. On success, it returns a 201 Created response with the created
    /// payment method. If the request is invalid, a 400 Bad Request response is returned.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the create payment method endpoint will be added.</param>
    public static void MapCreatePaymentMethodEndpoint(this WebApplication app)
    {
        app.MapPost("/api/payment-methods", async (CreatePaymentMethodCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/payment-methods/{result.Id}", result);
        })
        .WithName("CreatePaymentMethod")
        .WithTags("Payment Methods")
        .RequireAuthorization("AdminOnly")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<PaymentMethodDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
