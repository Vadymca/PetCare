namespace PetCare.Application.Features.Payments.LiqPay.LiqPayCallback;

using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles LiqPay payment callback commands by validating the callback signature and processing the payment data.
/// </summary>
/// <remarks>This handler is typically used in scenarios where LiqPay sends a callback to notify about payment
/// events. It verifies the callback's signature using the provided payment data and delegates processing to the
/// configured ILiqPayService. If the signature is invalid, a bad request result is returned; otherwise, the callback is
/// accepted. This class is intended to be used with MediatR's request/response pipeline.</remarks>
public sealed class HandleLiqPayCallbackCommandHandler
 : IRequestHandler<HandleLiqPayCallbackCommand, IResult>
{
    private readonly ILiqPayService liqPayService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HandleLiqPayCallbackCommandHandler"/> class using the specified LiqPay service.
    /// </summary>
    /// <param name="liqPayService">The service used to process LiqPay callbacks. Cannot be null.</param>
    public HandleLiqPayCallbackCommandHandler(ILiqPayService liqPayService)
    {
        this.liqPayService = liqPayService;
    }

    /// <summary>
    /// Processes a LiqPay callback request and returns an HTTP result indicating the outcome.
    /// </summary>
    /// <param name="request">The command containing the callback data and signature received from LiqPay.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>An HTTP result indicating whether the callback was processed successfully. Returns a bad request result if the
    /// signature is invalid; otherwise, returns an OK result.</returns>
    public async Task<IResult> Handle(HandleLiqPayCallbackCommand request, CancellationToken cancellationToken)
    {
        var ok = await this.liqPayService.ProcessCallbackAsync(request.Data, request.Signature, cancellationToken);
        if (!ok)
        {
            return Results.BadRequest("Invalid signature.");
        }

        return Results.Ok();
    }
}
