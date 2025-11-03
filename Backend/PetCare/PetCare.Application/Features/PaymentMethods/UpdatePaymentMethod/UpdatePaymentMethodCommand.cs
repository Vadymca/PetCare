namespace PetCare.Application.Features.PaymentMethods.UpdatePaymentMethod;

using System;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a command to update the name of an existing payment method.
/// </summary>
/// <param name="Id">The unique identifier of the payment method to update. Must correspond to an existing payment method.</param>
/// <param name="NewName">The new name to assign to the payment method. Cannot be null or empty.</param>
public sealed record UpdatePaymentMethodCommand(Guid Id, string NewName) : IRequest<PaymentMethodDto>;
