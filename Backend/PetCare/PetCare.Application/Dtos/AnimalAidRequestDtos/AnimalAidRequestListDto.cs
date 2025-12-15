namespace PetCare.Application.Dtos.AnimalAidRequestDtos;

using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Enums;
using System;

/// <summary>
/// DTO representing a summary of an animal aid request in a list.
/// </summary>
public sealed record AnimalAidRequestListDto(
Guid Id,
string Slug,
ShelterInfoDto? Shelter,
string Title,
string ShortDescription,
AidCategory Category,
decimal EstimatedCost,
decimal AllreadyDonated,
AidStatus Status,
string? Photo);
