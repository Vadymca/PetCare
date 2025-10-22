namespace PetCare.Api.Endpoints.Animals;

using PetCare.Domain.Enums;

/// <summary>
/// Represents the data required to create a new animal record, including identification, characteristics, health
/// information, and shelter details.
/// </summary>
/// <param name="Name">The name of the animal. Cannot be null or empty.</param>
/// <param name="BreedId">The unique identifier of the animal's breed.</param>
/// <param name="Birthday">The animal's date of birth, or null if unknown.</param>
/// <param name="Gender">The gender of the animal.</param>
/// <param name="Description">An optional description providing additional details about the animal.</param>
/// <param name="HealthConditions">A list of known health conditions affecting the animal, or null if none are specified.</param>
/// <param name="SpecialNeeds">A list of special needs or care requirements for the animal, or null if none are specified.</param>
/// <param name="Temperaments">A list of temperaments describing the animal's typical behavior, or null if not specified.</param>
/// <param name="Size">The size category of the animal.</param>
/// <param name="Photos">A list of URLs or file paths to photos of the animal, or null if not provided.</param>
/// <param name="Videos">A list of URLs or file paths to videos of the animal, or null if not provided.</param>
/// <param name="ShelterId">The unique identifier of the shelter responsible for the animal.</param>
/// <param name="Status">The current status of the animal, such as available for adoption or under care.</param>
/// <param name="CareCost">The estimated cost of care for the animal.</param>
/// <param name="AdoptionRequirements">Any specific requirements or conditions for adopting the animal, or null if none.</param>
/// <param name="MicrochipId">The animal's microchip identifier, or null if not applicable.</param>
/// <param name="Weight">The weight of the animal in kilograms, or null if unknown.</param>
/// <param name="Height">The height of the animal in centimeters, or null if unknown.</param>
/// <param name="Color">The primary color or markings of the animal, or null if not specified.</param>
/// <param name="IsSterilized">true if the animal has been sterilized; otherwise, false.</param>
/// <param name="IsUnderCare">true if the animal is currently under special care; otherwise, false.</param>
/// <param name="HaveDocuments">true if the animal has supporting documents (such as vaccination records); otherwise, false.</param>
public sealed record CreateAnimalBody(
    string Name,
    Guid BreedId,
    DateTime? Birthday,
    AnimalGender Gender,
    string? Description,
    List<string>? HealthConditions,
    List<string>? SpecialNeeds,
    List<AnimalTemperament>? Temperaments,
    AnimalSize Size,
    List<string>? Photos,
    List<string>? Videos,
    Guid ShelterId,
    AnimalStatus Status,
    AnimalCareCost CareCost,
    string? AdoptionRequirements,
    string? MicrochipId,
    float? Weight,
    float? Height,
    string? Color,
    bool IsSterilized = false,
    bool IsUnderCare = false,
    bool HaveDocuments = false);
