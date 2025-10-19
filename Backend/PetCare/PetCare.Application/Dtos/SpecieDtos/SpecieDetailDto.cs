namespace PetCare.Application.Dtos.SpecieDtos;

using System;

/// <summary>
/// Represents detailed information about a species, including its unique identifier and name.
/// </summary>
/// <param name="Id">The unique identifier for the species.</param>
/// <param name="Name">The name of the species.</param>
public sealed record SpecieDetailDto(Guid Id, string Name);
