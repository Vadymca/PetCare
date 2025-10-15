namespace PetCare.Application.Features.Animals.DeleteAnimal;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to delete an animal identified by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the animal to delete.</param>
public sealed record DeleteAnimalCommand(Guid Id) : IRequest<DeleteAnimalResponseDto>;
