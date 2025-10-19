namespace PetCare.Application.Features.Species.GetBreeds;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents a request to retrieve all breeds associated with a specified species.
/// </summary>
/// <param name="SpecieId">The unique identifier of the species for which breeds are to be retrieved.</param>
public sealed record GetBreedsCommand(Guid SpecieId) : IRequest<GetBreedsResponseDto>;
