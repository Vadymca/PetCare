namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents the result of creating a new recurring LiqPay subscription.
/// </summary>
public sealed record LiqPayRecurringResponseDto(
    string ProviderSubscriptionId,
    Guid PaymentMethodId,
    DateTime? NextChargeAt);
