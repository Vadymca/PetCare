namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents the request data for creating a new guardianship.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal to be placed under guardianship.</param>
public sealed record CreateGuardianshipRequestDto(Guid AnimalId);
