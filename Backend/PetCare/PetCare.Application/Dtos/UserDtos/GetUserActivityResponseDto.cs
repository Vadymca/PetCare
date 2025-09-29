namespace PetCare.Application.Dtos.UserDtos;

using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Dtos.EventDtos;
using System.Collections.Generic;

public sealed record GetUserActivityResponseDto(
 IReadOnlyList<AdoptionApplicationDto> AdoptionApplications,
 IReadOnlyList<EventDto> Events);
