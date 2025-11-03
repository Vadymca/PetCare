namespace PetCare.Api.Endpoints.Payments.LiqPay;

using System.Security.Claims;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.LiqPay.CreateLiqPayCheckout;

/// <summary>
/// Provides extension methods for mapping the LiqPay checkout endpoint to a web application.
/// </summary>
/// <remarks>This class contains static methods used to configure the routing for LiqPay payment checkout
/// operations. It is intended to be used during application startup to register the endpoint for creating new LiqPay
/// checkout sessions. The endpoint is mapped to POST /api/payments/liqpay/checkout and is tagged under "Payments" for
/// API documentation purposes.</remarks>
public static class LiqPayCheckoutEndpoint
{
    /// <summary>
    /// Maps the LiqPay checkout endpoint to the application's request pipeline, enabling clients to initiate a payment
    /// checkout process via a POST request.
    /// </summary>
    /// <remarks>The mapped endpoint accepts POST requests at '/api/payments/liqpay/checkout' and requires
    /// payment details in the request body. The endpoint supports both authenticated and anonymous users, and returns a
    /// checkout response on success or a 400 Bad Request on failure.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the LiqPay checkout endpoint will be mapped.</param>
    public static void MapLiqPayCheckoutEndpoint(this WebApplication app)
    {
        app.MapPost("/api/payments/liqpay/checkout", async (
            HttpContext context,
            IMediator mediator,
            CreateLiqPayCheckoutRequest request,
            CancellationToken cancellationToken) =>
        {
            // Визначаємо користувача з токена
            var userIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAuthenticated = Guid.TryParse(userIdStr, out var userId);

            var dto = new CreateLiqPayCheckoutDto(
                Amount: request.Amount,
                Currency: request.Currency,
                Description: request.Description,
                IsRecurring: request.IsRecurring,
                Scope: request.Scope,
                EntityId: request.EntityId,
                UserId: isAuthenticated ? userId : null,
                Anonymous: !isAuthenticated,
                PayerName: request.PayerName,
                PayerPhone: request.PayerPhone,
                PayerEmail: request.PayerEmail);

            var command = new CreateLiqPayCheckoutCommand(dto);
            var response = await mediator.Send(command, cancellationToken);
            return Results.Ok(response);
        })
        .WithName("CreateLiqPayCheckout")
        .WithTags("Payments")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<LiqPayCheckoutResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
