namespace PetCare.Application.Dtos.UserDtos;

using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Dtos.ShelterDtos;
using System.Collections.Generic;

public sealed record GetUserSubscriptionsResponseDto(
 IReadOnlyList<ShelterDto> Shelters,
 IReadOnlyList<AnimalListDto> Animals);
