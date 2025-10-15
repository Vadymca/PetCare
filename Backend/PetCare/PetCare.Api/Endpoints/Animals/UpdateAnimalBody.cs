namespace PetCare.Api.Endpoints.Animals;

using PetCare.Domain.Enums;

/// <summary>
/// Represents the data required to update the details of an animal in the system.
/// </summary>
/// <param name="Name">The updated name of the animal, or null to leave unchanged.</param>
/// <param name="Birthday">The updated date of birth of the animal, or null to leave unchanged.</param>
/// <param name="Gender">The updated gender of the animal, or null to leave unchanged.</param>
/// <param name="Description">The updated description of the animal, or null to leave unchanged.</param>
/// <param name="Status">The updated status of the animal, such as available or adopted, or null to leave unchanged.</param>
/// <param name="AdoptionRequirements">The updated adoption requirements for the animal, or null to leave unchanged.</param>
/// <param name="MicrochipId">The updated microchip identifier for the animal, or null to leave unchanged.</param>
/// <param name="Weight">The updated weight of the animal in kilograms, or null to leave unchanged.</param>
/// <param name="Height">The updated height of the animal in centimeters, or null to leave unchanged.</param>
/// <param name="Color">The updated color description of the animal, or null to leave unchanged.</param>
/// <param name="IsSterilized">true if the animal is sterilized; otherwise, false. Null to leave unchanged.</param>
/// <param name="HaveDocuments">true if the animal has supporting documents; otherwise, false. Null to leave unchanged.</param>
/// <param name="HealthConditions">A list of updated health conditions for the animal, or null to leave unchanged.</param>
/// <param name="SpecialNeeds">A list of updated special needs for the animal, or null to leave unchanged.</param>
/// <param name="Temperaments">A list of updated temperament traits for the animal, or null to leave unchanged.</param>
/// <param name="Size">The updated size category of the animal, or null to leave unchanged.</param>
/// <param name="CareCost">The updated estimated care cost for the animal, or null to leave unchanged.</param>
public sealed record UpdateAnimalBody(
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
AnimalCareCost? CareCost);
