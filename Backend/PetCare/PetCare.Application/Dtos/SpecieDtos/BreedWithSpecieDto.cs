namespace PetCare.Application.Dtos.SpecieDtos;

using System;

/// <summary>
/// Represents a breed along with its associated species information for data transfer operations.
/// </summary>
/// <param name="Id">The unique identifier of the breed.</param>
/// <param name="Name">The name of the breed.</param>
/// <param name="Description">An optional description providing additional details about the breed, or null if not specified.</param>
/// <param name="Specie">A brief data transfer object containing information about the species to which the breed belongs.</param>
public sealed record BreedWithSpecieDto(
 Guid Id,
 string Name,
 string? Description,
 SpecieBriefDto Specie);
