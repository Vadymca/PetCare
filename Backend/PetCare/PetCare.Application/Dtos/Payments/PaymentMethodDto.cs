namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents a lightweight data transfer object for a payment method.
/// </summary>
/// <param name="Id">The unique identifier of the payment method.</param>
/// <param name="Name">The name of the payment method.</param>
public sealed record PaymentMethodDto(Guid Id, string Name);
