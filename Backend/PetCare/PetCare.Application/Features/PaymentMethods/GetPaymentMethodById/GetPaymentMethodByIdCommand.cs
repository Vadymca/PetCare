namespace PetCare.Application.Features.PaymentMethods.GetPaymentMethodById;

using System;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a request to retrieve a payment method by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the payment method to retrieve.</param>
public sealed record GetPaymentMethodByIdCommand(Guid Id) : IRequest<PaymentMethodDto>;
