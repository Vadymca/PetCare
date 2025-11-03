namespace PetCare.Application.Features.Payments.LiqPay.CheckPaymentStatus;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the request for checking LiqPay payment status by order ID.
/// </summary>
public sealed class CheckLiqPayPaymentStatusCommandHandler
    : IRequestHandler<CheckLiqPayPaymentStatusCommand, LiqPayPaymentStatusResponseDto>
{
    private readonly ILiqPayClient liqPayClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckLiqPayPaymentStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="liqPayClient">The client used to communicate with LiqPay API.</param>
    public CheckLiqPayPaymentStatusCommandHandler(ILiqPayClient liqPayClient)
    {
        this.liqPayClient = liqPayClient ?? throw new ArgumentNullException(nameof(liqPayClient));
    }

    /// <inheritdoc/>
    public async Task<LiqPayPaymentStatusResponseDto> Handle(
        CheckLiqPayPaymentStatusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await this.liqPayClient.RequestStatusAsync(request.Request.OrderId, cancellationToken);

        string orderId = request.Request.OrderId;
        string status = result.TryGetProperty("status", out var s) ? s.GetString() ?? "unknown" : "unknown";
        string action = result.TryGetProperty("action", out var a) ? a.GetString() ?? "pay" : "pay";
        decimal amount = result.TryGetProperty("amount", out var am) ? am.GetDecimal() : 0;
        string currency = result.TryGetProperty("currency", out var c) ? c.GetString() ?? "UAH" : "UAH";
        string? description = result.TryGetProperty("description", out var d) ? d.GetString() : null;
        DateTime? createdDate = result.TryGetProperty("create_date", out var cd)
            ? DateTimeOffset.FromUnixTimeSeconds(cd.GetInt64()).UtcDateTime
            : null;
        DateTime? endDate = result.TryGetProperty("end_date", out var ed)
            ? DateTimeOffset.FromUnixTimeSeconds(ed.GetInt64()).UtcDateTime
            : null;

        return new LiqPayPaymentStatusResponseDto(
            OrderId: orderId,
            Status: status,
            Action: action,
            Amount: amount,
            Currency: currency,
            Description: description,
            CreatedDate: createdDate,
            EndDate: endDate);
    }
}
