namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>Represents a guardianship summary for the user.</summary>
public sealed record MyGuardianshipDto(
    Guid Id,
    Guid AnimalId,
    string AnimalName,
    string AnimalSlug,
    DateTime StartDate,
    string Status);
