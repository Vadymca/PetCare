namespace PetCare.Application.Features.PaymentMethods.CreatePaymentMethod;

using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a request to create a new payment method with the specified name.
/// </summary>
/// <param name="Name">The name of the payment method to be created. Cannot be null or empty.</param>
public sealed record CreatePaymentMethodCommand(string Name) : IRequest<PaymentMethodDto>;
