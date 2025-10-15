namespace PetCare.Application.Features.Animals.UpdateAnimal;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a command to update the details of an existing animal in the system.
/// </summary>
/// <remarks>Any parameter set to null will leave the corresponding animal property unchanged. Only the specified
/// fields will be updated.</remarks>
/// <param name="Id">The unique identifier of the animal to update.</param>
/// <param name="Name">The new name of the animal, or null to leave unchanged.</param>
/// <param name="Birthday">The updated birth date of the animal, or null to leave unchanged.</param>
/// <param name="Gender">The updated gender of the animal, or null to leave unchanged.</param>
/// <param name="Description">The updated description of the animal, or null to leave unchanged.</param>
/// <param name="Status">The updated status of the animal, or null to leave unchanged.</param>
/// <param name="AdoptionRequirements">The updated adoption requirements for the animal, or null to leave unchanged.</param>
/// <param name="MicrochipId">The updated microchip identifier for the animal, or null to leave unchanged.</param>
/// <param name="Weight">The updated weight of the animal in kilograms, or null to leave unchanged.</param>
/// <param name="Height">The updated height of the animal in centimeters, or null to leave unchanged.</param>
/// <param name="Color">The updated color description of the animal, or null to leave unchanged.</param>
/// <param name="IsSterilized">A value indicating whether the animal is sterilized, or null to leave unchanged.</param>
/// <param name="HaveDocuments">A value indicating whether the animal has supporting documents, or null to leave unchanged.</param>
/// <param name="HealthConditions">A list of updated health conditions for the animal, or null to leave unchanged.</param>
/// <param name="SpecialNeeds">A list of updated special needs for the animal, or null to leave unchanged.</param>
/// <param name="Temperaments">A list of updated temperaments for the animal, or null to leave unchanged.</param>
/// <param name="Size">The updated size classification of the animal, or null to leave unchanged.</param>
/// <param name="CareCost">The updated care cost information for the animal, or null to leave unchanged.</param>
public sealed record UpdateAnimalCommand(
Guid Id,
string? Name,
DateTime? Birthday,
AnimalGender? Gender,
string? Description,
AnimalStatus? Status,
string? AdoptionRequirements,
string? MicrochipId,
float? Weight,
float? Height,
string? Color,
bool? IsSterilized,
bool? HaveDocuments,
List<string>? HealthConditions,
List<string>? SpecialNeeds,
List<AnimalTemperament>? Temperaments,
AnimalSize? Size,
AnimalCareCost? CareCost)
    : IRequest<AnimalDto>;
