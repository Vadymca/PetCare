namespace PetCare.Application.Features.Breeds.UpdateBreed;

using System;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents a request to update the details of an existing breed, including its name and description.
/// </summary>
/// <param name="Id">The unique identifier of the breed to update.</param>
/// <param name="Name">The new name for the breed, or null to leave the name unchanged.</param>
/// <param name="Description">The new description for the breed, or null to leave the description unchanged.</param>
public sealed record UpdateBreedCommand(
Guid Id,
string? Name,
string? Description) : IRequest<BreedWithSpecieDto>;
