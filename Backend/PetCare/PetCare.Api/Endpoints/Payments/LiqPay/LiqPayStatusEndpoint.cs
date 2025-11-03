namespace PetCare.Api.Endpoints.Payments.LiqPay;

using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Features.Payments.LiqPay.CheckPaymentStatus;

/// <summary>
/// Endpoint for checking the current status of a LiqPay payment.
/// </summary>
public static class LiqPayStatusEndpoint
{
    /// <summary>
    /// Maps POST /api/payments/liqpay/status to check the payment state.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    public static void MapLiqPayStatusEndpoint(this WebApplication app)
    {
        app.MapPost("/api/payments/liqpay/status", async (
            IMediator mediator,
            LiqPayPaymentStatusRequestDto dto,
            CancellationToken cancellationToken) =>
        {
            var command = new CheckLiqPayPaymentStatusCommand(dto);
            var response = await mediator.Send(command, cancellationToken);
            return Results.Ok(response);
        })
        .WithName("CheckLiqPayPaymentStatus")
        .WithTags("Payments")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<LiqPayPaymentStatusResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
