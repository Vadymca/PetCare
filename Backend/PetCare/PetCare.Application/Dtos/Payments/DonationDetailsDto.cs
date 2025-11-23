namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents the details of a donation, including its identifier, amount, currency, and optional purpose.
/// </summary>
/// <param name="Id">The unique identifier of the donation.</param>
/// <param name="Amount">The monetary amount of the donation.</param>
/// <param name="Currency">The ISO 4217 currency code representing the currency of the donation. Cannot be null.</param>
/// <param name="Purpose">An optional description specifying the purpose of the donation, or null if not specified.</param>
public sealed record DonationDetailsDto(
     Guid Id,
     decimal Amount,
     string Currency,
     string? Purpose,
     string Status,
     string? TransactionId,
     string? TargetEntity,
     Guid? TargetEntityId);
