namespace PetCare.Application.Features.Animals.CreateAnimal;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a command to create a new animal record with detailed information, including identification,
/// characteristics, health, and care requirements.
/// </summary>
/// <param name="UserId">The unique identifier of the user creating the animal record.</param>
/// <param name="Name">The name of the animal. Cannot be null or empty.</param>
/// <param name="BreedId">The unique identifier of the animal's breed.</param>
/// <param name="Birthday">The animal's date of birth, or null if unknown.</param>
/// <param name="Gender">The gender of the animal.</param>
/// <param name="Description">An optional description providing additional details about the animal.</param>
/// <param name="HealthConditions">A list of health conditions affecting the animal, or null if none are specified.</param>
/// <param name="SpecialNeeds">A list of special needs or accommodations required by the animal, or null if none.</param>
/// <param name="Temperaments">A list of temperaments describing the animal's typical behavior, or null if not specified.</param>
/// <param name="Size">The size category of the animal.</param>
/// <param name="Photos">A list of URLs or file paths to photos of the animal, or null if none are provided.</param>
/// <param name="Videos">A list of URLs or file paths to videos of the animal, or null if none are provided.</param>
/// <param name="ShelterId">The unique identifier of the shelter where the animal is located.</param>
/// <param name="Status">The current status of the animal, such as available or adopted.</param>
/// <param name="CareCost">The estimated cost of care for the animal.</param>
/// <param name="AdoptionRequirements">Optional requirements or criteria for adopting the animal.</param>
/// <param name="MicrochipId">The animal's microchip identifier, or null if not applicable.</param>
/// <param name="Weight">The weight of the animal in kilograms, or null if unknown.</param>
/// <param name="Height">The height of the animal in centimeters, or null if unknown.</param>
/// <param name="Color">The color or primary markings of the animal, or null if not specified.</param>
/// <param name="IsSterilized">true if the animal has been sterilized; otherwise, false.</param>
/// <param name="IsUnderCare">true if the animal is currently under special care; otherwise, false.</param>
/// <param name="HaveDocuments">true if the animal has supporting documents (such as vaccination records); otherwise, false.</param>
public sealed record CreateAnimalCommand(
    Guid UserId,
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
    bool HaveDocuments = false) : IRequest<AnimalDto>;