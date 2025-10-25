namespace PetCare.Application.Dtos.AnimalDtos;

using System;
using System.Collections.Generic;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a data transfer object containing detailed information about an animal available for adoption, including
/// identification, characteristics, health status, and shelter details.
/// </summary>
/// <param name="Id">The unique identifier of the animal.</param>
/// <param name="Slug">A URL-friendly string that uniquely identifies the animal for web or API access.</param>
/// <param name="Name">The name of the animal.</param>
/// <param name="Birthday">The animal's date of birth, or null if unknown. The format is typically ISO 8601 (e.g., "YYYY-MM-DD").</param>
/// <param name="Gender">The gender of the animal. Common values include "Male" or "Female".</param>
/// <param name="Description">A textual description providing additional details about the animal, or null if not specified.</param>
/// <param name="HealthConditions">A list of health conditions affecting the animal. The list is empty if there are no known health conditions.</param>
/// <param name="SpecialNeeds">A list of special needs or care requirements for the animal. The list is empty if there are no special needs.</param>
/// <param name="Size">The size category of the animal, such as "Small", "Medium", or "Large".</param>
/// <param name="Temperaments">A list of temperament descriptors that characterize the animal's typical behavior.</param>
/// <param name="Photos">A list of URLs or file paths to photos of the animal. The list is empty if no photos are available.</param>
/// <param name="Status">The current adoption status of the animal, such as "Available", "Adopted", or "Pending".</param>
/// <param name="CareCost">An object representing the estimated cost of care for the animal.</param>
/// <param name="AdoptionRequirements">Additional requirements or conditions for adopting the animal, or null if there are none.</param>
/// <param name="MicrochipId">The animal's microchip identifier, or null if the animal is not microchipped.</param>
/// <param name="Weight">The weight of the animal in kilograms, or null if not specified.</param>
/// <param name="Height">The height of the animal in centimeters, or null if not specified.</param>
/// <param name="Color">The primary color or color pattern of the animal, or null if not specified.</param>
/// <param name="IsSterilized">true if the animal has been sterilized (spayed or neutered); otherwise, false.</param>
/// <param name="IsUnderCare">true if the animal is currently under the care of the shelter or organization; otherwise, false.</param>
/// <param name="HaveDocuments">true if official documents (such as vaccination records or pedigree papers) are available for the animal; otherwise,
/// false.</param>
/// <param name="CreatedAt">The date and time when the animal record was created, in UTC.</param>
/// <param name="UpdatedAt">The date and time when the animal record was last updated, in UTC.</param>
/// <param name="Specie">An object containing information about the animal's species.</param>
/// <param name="Shelter">An object containing information about the shelter or organization responsible for the animal.</param>
/// <param name="Breed">An object containing information about the animal's breed.</param>
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
 bool IsUnderCare,
 bool HaveDocuments,
 DateTime CreatedAt,
 DateTime UpdatedAt,
 SpecieDto Species,
 ShelterInfoDto Shelter,
 BreedDto Breed);
