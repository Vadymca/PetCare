namespace PetCare.Application.Features.Animals.UpdateAnimal;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Enums;
using System;
using System.Collections.Generic;

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
