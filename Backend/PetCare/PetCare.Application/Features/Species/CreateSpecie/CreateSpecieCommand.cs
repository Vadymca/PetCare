namespace PetCare.Application.Features.Species.CreateSpecie;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Command to create a new species.
/// </summary>
/// <param name="Name">The name of the new species.</param>
public sealed record CreateSpecieCommand(string Name) : IRequest<SpecieListDto>;
