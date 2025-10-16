namespace PetCare.Application.Dtos.ShelterDtos;

using System.Collections.Generic;

/// <summary>
/// Represents a paginated response containing a list of shelters.
/// </summary>
/// <param name="Shelters">The list of shelters in the current page.</param>
/// <param name="TotalCount">The total number of shelters.</param>
public sealed record GetSheltersResponseDto(IReadOnlyList<ShelterListDto> Shelters, int TotalCount);
