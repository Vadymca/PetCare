namespace PetCare.Application.Features.Payments.LiqPay.CheckPaymentStatus;

using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Command to check the current status of a LiqPay payment by order ID.
/// </summary>
/// <param name="Request">The DTO containing the order ID.</param>
public sealed record CheckLiqPayPaymentStatusCommand(LiqPayPaymentStatusRequestDto Request)
    : IRequest<LiqPayPaymentStatusResponseDto>;
