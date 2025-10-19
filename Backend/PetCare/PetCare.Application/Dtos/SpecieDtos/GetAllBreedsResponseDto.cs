namespace PetCare.Application.Dtos.SpecieDtos;

using System.Collections.Generic;

/// <summary>
/// Represents the response for retrieving all breeds in the system.
/// </summary>
/// <param name="Breeds">The list of breeds.</param>
/// <param name="TotalCount">The total number of breeds available.</param>
public sealed record GetAllBreedsResponseDto(
    IReadOnlyList<BreedWithSpecieDto> Breeds,
    int TotalCount);
