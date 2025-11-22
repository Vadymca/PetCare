namespace PetCare.Application.Dtos.Payments;

using System;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>Represents a guardianship summary for the user.</summary>
public sealed record MyGuardianshipDto(
     Guid Id,
     DateTime StartDate,
     DateTime GraceUntil,
     string Status,
     AnimalDto Animal,
     PaymentSubscriptionDto? PaymentSubscription);