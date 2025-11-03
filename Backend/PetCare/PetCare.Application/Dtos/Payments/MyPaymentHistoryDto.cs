namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>Represents a user's payment history entry.</summary>
public sealed record MyPaymentHistoryDto(
    Guid Id,
    decimal Amount,
    string Currency,
    string Status,
    bool Recurring,
    string Purpose,
    string TargetEntity,
    Guid? TargetEntityId,
    DateTime DonationDate);
