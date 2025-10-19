namespace PetCare.Application.Dtos.SpecieDtos;

using System.Collections.Generic;

/// <summary>
/// Represents the response for retrieving breeds of a species.
/// </summary>
/// <param name="Breeds">The list of breeds.</param>
/// <param name="TotalCount">Total number of breeds.</param>
public sealed record GetBreedsResponseDto(
   IReadOnlyList<BreedListDto> Breeds,
   int TotalCount);
