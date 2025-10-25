namespace PetCare.Application.Features.Animals.GetAnimals;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a request to retrieve a paginated list of animals, optionally filtered by various criteria such as size,
/// gender, age, care cost, sterilization status, shelter, species, breed, status, and search term.
/// </summary>
/// <param name="Page">The page number of results to retrieve. Must be greater than or equal to 1.</param>
/// <param name="PageSize">The maximum number of animals to include in a single page of results. Must be greater than 0.</param>
/// <param name="Sizes">A collection of animal sizes to filter the results by. If null, animals of all sizes are included.</param>
/// <param name="Genders">A collection of animal genders to filter the results by. If null, animals of all genders are included.</param>
/// <param name="MinAge">The minimum age, in years, of animals to include in the results. If null, no minimum age filter is applied.</param>
/// <param name="MaxAge">The maximum age, in years, of animals to include in the results. If null, no maximum age filter is applied.</param>
/// <param name="CareCosts">A collection of care cost categories to filter the results by. If null, animals of all care cost categories are
/// included.</param>
/// <param name="IsSterilized">If set, filters animals by sterilization status. When <see langword="true"/>, only sterilized animals are included;
/// when <see langword="false"/>, only non-sterilized animals are included.</param>
/// <param name="IsUndercare">If set, filters animals by whether they are currently under care. When <see langword="true"/>, only animals under
/// care are included; when <see langword="false"/>, only animals not under care are included.</param>
/// <param name="ShelterId">The unique identifier of the shelter to filter animals by. If null, animals from all shelters are included.</param>
/// <param name="Statuses">A collection of animal statuses to filter the results by. If null, animals of all statuses are included.</param>
/// <param name="SpecieId">The unique identifier of the species to filter animals by. If null, animals of all species are included.</param>
/// <param name="BreedId">The unique identifier of the breed to filter animals by. If null, animals of all breeds are included.</param>
/// <param name="Search">A search term to filter animals by name or other searchable fields. If null or empty, no search filtering is
/// applied.</param>
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
    string? Search = null,
    string? AnimalTypeFilter = null)
    : IRequest<GetAnimalsResponseDto>;
