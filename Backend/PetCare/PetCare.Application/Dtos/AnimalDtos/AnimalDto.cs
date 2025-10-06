namespace PetCare.Application.Dtos.AnimalDtos;

using PetCare.Domain.Enums;
using System;
using System.Collections.Generic;

public sealed record AnimalDto(
 Guid Id,
 string Slug,
 string Name,
 string? Birthday,
 string Gender,
 string? Description,
 IReadOnlyList<string> HealthConditions,
 IReadOnlyList<string> SpecialNeeds,
 string Size,
 IReadOnlyList<string> Temperaments,
 IReadOnlyList<string> Photos,
 string Status,
 AnimalCareCost CareCost,
string? AdoptionRequirements,
 string? MicrochipId,
 float? Weight,
 float? Height,
 string? Color,
 bool IsSterilized,
 bool HaveDocuments,
 DateTime CreatedAt,
 DateTime UpdatedAt,
 SpecieDto Specie,
 ShelterInfoDto Shelter,
 BreedDto Breed);
