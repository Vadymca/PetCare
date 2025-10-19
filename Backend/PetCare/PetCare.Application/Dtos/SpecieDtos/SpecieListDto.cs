namespace PetCare.Application.Dtos.SpecieDtos;

using System;

/// <summary>
/// Represents a summary of an animal species for list views.
/// </summary>
public sealed record SpecieListDto(
    Guid Id,
    string Name);
