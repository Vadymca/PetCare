namespace PetCare.Application.Features.Animals.CreateAnimal;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Enums;

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
    bool IsSterilized,
    bool IsUnderCare,
    bool HaveDocuments
) : IRequest<AnimalDto>;