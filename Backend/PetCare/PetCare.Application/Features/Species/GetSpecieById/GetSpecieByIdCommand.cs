namespace PetCare.Application.Features.Species.GetSpecieById;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents a request to retrieve a species by its ID.
/// </summary>
/// <param name="Id">The unique identifier of the species.</param>
public sealed record GetSpecieByIdCommand(Guid Id) : IRequest<SpecieDetailDto>;
