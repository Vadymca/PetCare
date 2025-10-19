namespace PetCare.Application.Features.Breeds.GetBreedById;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents a request to retrieve a breed by its unique identifier.
/// </summary>
/// <param name="BreedId">The unique identifier of the breed to retrieve.</param>
public sealed record GetBreedByIdCommand(Guid BreedId) : IRequest<BreedWithSpecieDto>;
