namespace PetCare.Application.Dtos.Payments;

using System;
using System.Collections.Generic;
using PetCare.Domain.Enums;

/// <summary>
/// Represents the data required to initialize a LiqPay checkout process.
/// </summary>
/// <param name="Amount">The payment amount to be charged (must be greater than zero).</param>
/// <param name="Currency">The currency of the payment, typically "UAH".</param>
/// <param name="Description">A textual description of the payment purpose.</param>
/// <param name="IsRecurring">Indicates whether the payment should create a recurring LiqPay subscription.</param>
/// <param name="Scope">Specifies the scope of the payment — Global, AidRequest, or Guardianship.</param>
/// <param name="EntityId">The identifier of the related entity (e.g., GuardianshipId, AidRequestId).</param>
/// <param name="UserId">The identifier of the user performing the payment.</param>
/// <param name="Anonymous">If true, the payment will be recorded as anonymous.</param>
/// <param name="PayerName">The payer’s full name (optional, used for receipts or personalized thank-you messages).</param>
/// <param name="PayerPhone">The payer’s phone number (optional, used for contact or payment verification).</param>
/// <param name="PayerEmail">The payer’s email address (optional, used for sending payment confirmations).</param>
public sealed record CreateLiqPayCheckoutDto(
    decimal Amount,
    string Currency = "UAH",
    string? Description = null,
    bool IsRecurring = false, // false = одноразово; true = LiqPay subscribe
    SubscriptionScope? Scope = null, // Global/AidRequest/Guardianship
    Guid? EntityId = null, // наприклад, GuardianshipId
    Guid? UserId = null,
    bool Anonymous = false,
    string? PayerName = null,
    string? PayerPhone = null,
    string? PayerEmail = null);
