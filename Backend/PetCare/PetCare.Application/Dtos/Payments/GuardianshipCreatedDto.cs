namespace PetCare.Application.Dtos.Payments;

using System;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents the data returned after a new guardianship is successfully created.
/// </summary>
public sealed record GuardianshipCreatedDto(
    Guid Id,
    DateTime StartDate,
    DateTime GraceUntil,
    string Status,
    AnimalDto Animal,
    PaymentSubscriptionDto? PaymentSubscription);