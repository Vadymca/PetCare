namespace PetCare.Application.Features.Breeds.DeleteBreed;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents a request to delete a breed identified by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the breed to delete.</param>
public sealed record DeleteBreedCommand(Guid Id) : IRequest<DeleteBreedResponseDto>;
