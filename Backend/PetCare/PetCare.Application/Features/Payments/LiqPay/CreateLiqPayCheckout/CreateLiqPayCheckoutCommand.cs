namespace PetCare.Application.Features.Payments.LiqPay.CreateLiqPayCheckout;

using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Command to initiate a LiqPay checkout process.
/// </summary>
/// <param name="Request">The DTO containing all required payment parameters.</param>
public sealed record CreateLiqPayCheckoutCommand(CreateLiqPayCheckoutDto Request)
    : IRequest<LiqPayCheckoutResponseDto>;
