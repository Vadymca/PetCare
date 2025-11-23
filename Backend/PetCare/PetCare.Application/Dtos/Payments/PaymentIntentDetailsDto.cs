namespace PetCare.Application.Dtos.Payments;

using System;
using PetCare.Domain.Enums;

/// <summary>
/// Represents the details of a payment intent, including order information, status, amount, currency, and related
/// entities such as donation, guardianship, or subscription details.
/// </summary>
/// <param name="OrderId">The unique identifier of the order associated with the payment intent.</param>
/// <param name="Status">The current status of the payment intent.</param>
/// <param name="Scope">The scope of the subscription, if applicable. May be null if the payment is not related to a subscription.</param>
/// <param name="ScopeId">The unique identifier of the scope, if applicable. May be null if not associated with a specific scope.</param>
/// <param name="UserId">The unique identifier of the user associated with the payment intent, or null if the payment is anonymous or not
/// user-specific.</param>
/// <param name="Amount">The total amount to be charged for the payment intent, in the specified currency.</param>
/// <param name="Currency">The ISO currency code representing the currency of the payment amount.</param>
/// <param name="IsRecurring">A value indicating whether the payment intent is for a recurring payment, such as a subscription.</param>
/// <param name="Anonymous">A value indicating whether the payment was made anonymously. If <see langword="true"/>, user information may be
/// omitted.</param>
/// <param name="Donation">The details of the donation associated with the payment intent, or null if not applicable.</param>
/// <param name="Guardianship">The details of the guardianship associated with the payment intent, or null if not applicable.</param>
/// <param name="Subscription">The details of the subscription associated with the payment intent, or null if not applicable.</param>
public sealed record PaymentIntentDetailsDto(
    string OrderId,
    PaymentIntentStatus Status,
    string? ProviderPaymentId,
    SubscriptionScope? Scope,
    Guid? ScopeId,
    Guid? UserId,
    decimal Amount,
    string Currency,
    bool IsRecurring,
    bool Anonymous,
    DonationDetailsDto? Donation,
    GuardianshipDetailsDto? Guardianship,
    SubscriptionDetailsDto? Subscription,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string? Message);
