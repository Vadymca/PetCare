namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents payment subscription details associated with guardianship.
/// </summary>
/// <param name="Id">The unique identifier of the subscription.</param>
/// <param name="Amount">Payment amount.</param>
/// <param name="Currency">Payment currency (e.g. "UAH").</param>
/// <param name="NextPaymentDate">When the next payment is expected.</param>
/// <param name="Status">Subscription status (Active, Canceled, Paused).</param>
/// <param name="IsOverdue">Indicates whether payment is overdue.</param>
public sealed record PaymentSubscriptionDto(
    Guid Id,
    decimal Amount,
    string Currency,
    DateTime? NextChargeAt,
    string Status,
    string? ProviderSubscriptionId,
    bool IsOverdue);
