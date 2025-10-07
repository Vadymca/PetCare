namespace PetCare.Application.Features.Animals.GetAnimals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Enums;
using System;

public sealed record GetAnimalsCommand(
    int Page = 1,
    int PageSize = 20,
    IEnumerable<AnimalSize>? Sizes = null,
    IEnumerable<AnimalGender>? Genders = null,
    int? MinAge = null,
    int? MaxAge = null,
    IEnumerable<AnimalCareCost>? CareCosts = null,
    bool? IsSterilized = null,
    bool? IsUndercare = null,
    Guid? ShelterId = null,
    IEnumerable<AnimalStatus>? Statuses = null,
    Guid? SpecieId = null,
    Guid? BreedId = null,
    string? Search = null)
    : IRequest<GetAnimalsResponseDto>;
