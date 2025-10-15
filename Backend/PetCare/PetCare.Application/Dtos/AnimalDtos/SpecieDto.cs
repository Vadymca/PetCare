namespace PetCare.Application.Dtos.AnimalDtos;

using System;

/// <summary>
/// Represents a data transfer object containing information about a species.
/// </summary>
/// <param name="Id">The unique identifier of the species.</param>
/// <param name="Name">The name of the species.</param>
public sealed record SpecieDto(
     Guid Id,
     string Name);
