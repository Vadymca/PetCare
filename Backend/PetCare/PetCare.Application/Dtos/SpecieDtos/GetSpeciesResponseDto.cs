namespace PetCare.Application.Dtos.SpecieDtos;

using System.Collections.Generic;

/// <summary>
/// Represents the response for a list of animal species.
/// </summary>
public sealed record GetSpeciesResponseDto(
    IReadOnlyList<SpecieListDto> Species);
