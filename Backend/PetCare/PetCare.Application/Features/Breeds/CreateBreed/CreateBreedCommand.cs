namespace PetCare.Application.Features.Breeds.CreateBreed;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Command to create a new breed.
/// </summary>
/// <param name="Name">The name of the breed.</param>
/// <param name="Description">Optional description of the breed.</param>
/// <param name="SpecieId">The ID of the specie this breed belongs to.</param>
public sealed record CreateBreedCommand(string Name, string? Description, Guid SpecieId) : IRequest<BreedWithSpecieDto>;
