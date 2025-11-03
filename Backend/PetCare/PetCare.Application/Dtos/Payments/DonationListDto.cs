namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents a simplified view of a donation record for display in public or project-specific payment listings.
/// </summary>
public sealed record DonationListDto(
    Guid Id,
    string? UserName,
    string? UserPhotoUrl,
    decimal Amount,
    string Currency,
    DateTime DonationDate,
    bool IsAnonymous);
