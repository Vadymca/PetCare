namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents the details of a guardianship relationship for an animal.
/// </summary>
/// <param name="Id">The unique identifier for the guardianship record.</param>
/// <param name="AnimalId">The unique identifier of the animal associated with the guardianship.</param>
/// <param name="AnimalName">The name of the animal associated with the guardianship.</param>
public sealed record GuardianshipDetailsDto(
    Guid Id,
    Guid AnimalId,
    string AnimalName,
    string Status,
    DateTime StartDate,
    DateTime? GraceUntil);
