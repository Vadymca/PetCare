namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents the response returned after deleting a guardianship.
/// </summary>
/// <param name="Id">The ID of the deleted guardianship.</param>
/// <param name="HadSubscription">Indicates whether the guardianship had a linked subscription.</param>
/// <param name="CanceledSubscription">Indicates whether the subscription was canceled during deletion.</param>
/// <param name="Message">A human-friendly message describing the result.</param>
public sealed record GuardianshipDeletedDto(
    Guid Id,
    bool HadSubscription,
    bool CanceledSubscription,
    string Message);
