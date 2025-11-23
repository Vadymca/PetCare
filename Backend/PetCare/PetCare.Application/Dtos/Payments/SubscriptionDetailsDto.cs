namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents the details of a subscription, including its unique identifier, amount, and currency.
/// </summary>
/// <param name="Id">The unique identifier of the subscription.</param>
/// <param name="Amount">The monetary amount associated with the subscription.</param>
/// <param name="Currency">The ISO currency code representing the currency of the subscription amount.</param>
public sealed record SubscriptionDetailsDto(
    Guid Id,
    decimal Amount,
    string Currency,
    string Provider,
    string ProviderSubscriptionId,
    string Status,
    DateTime? LastChargeAt,
    DateTime? NextChargeAt);
