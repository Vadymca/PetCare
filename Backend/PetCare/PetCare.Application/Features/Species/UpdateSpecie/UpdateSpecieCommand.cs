namespace PetCare.Application.Features.Species.UpdateSpecie;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents a request to update the details of an existing species.
/// </summary>
/// <param name="Id">The unique identifier of the species to update.</param>
/// <param name="Name">The new name to assign to the species.</param>
public sealed record UpdateSpecieCommand(
 Guid Id,
 string Name) : IRequest<SpecieDetailDto>;
