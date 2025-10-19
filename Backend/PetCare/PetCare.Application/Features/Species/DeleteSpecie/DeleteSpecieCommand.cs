namespace PetCare.Application.Features.Species.DeleteSpecie;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents a request to delete a species identified by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the species to delete.</param>
public sealed record DeleteSpecieCommand(Guid Id) : IRequest<DeleteSpecieResponseDto>;