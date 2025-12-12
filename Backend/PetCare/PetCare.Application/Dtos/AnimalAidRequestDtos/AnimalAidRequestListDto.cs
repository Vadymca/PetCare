namespace PetCare.Application.Dtos.AnimalAidRequestDtos;

using System;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// DTO representing a summary of an animal aid request in a list.
/// </summary>
public sealed record AnimalAidRequestListDto(
Guid Id,
string Slug,
ShelterInfoDto? Shelter,
string Title,
string ShortDescription,
string Category,
decimal EstimatedCost,
decimal AllreadyDonated,
string? Photo);
