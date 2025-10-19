namespace PetCare.Application.Features.Breeds.GetBreedsBySpecie;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Command to get all breeds for a specific specie.
/// </summary>
/// <param name="SpecieId">The ID of the specie.</param>
public sealed record GetBreedsBySpecieCommand(Guid SpecieId) : IRequest<GetBreedsResponseDto>;
