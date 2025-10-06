namespace PetCare.Application.Dtos.AnimalDtos;

using System.Collections.Generic;

public sealed record GetAnimalsResponseDto(
     IReadOnlyList<AnimalListDto> Animals,
     int TotalCount);
