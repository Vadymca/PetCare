namespace PetCare.Application.Features.PaymentMethods.DeletePaymentMethod;

using System;
using MediatR;

/// <summary>
/// Represents a request to delete a payment method identified by its unique ID.
/// </summary>
/// <param name="Id">The unique identifier of the payment method to be deleted. Must correspond to an existing payment method.</param>
public sealed record DeletePaymentMethodCommand(Guid Id) : IRequest;
