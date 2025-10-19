namespace PetCare.Application.Features.Species.GetSpecies;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Request to get all species (no pagination).
/// </summary>
public sealed record GetSpeciesCommand() : IRequest<GetSpeciesResponseDto>;
