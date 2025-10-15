namespace PetCare.Application.Dtos.AnimalDtos;

using System;

/// <summary>
/// Represents a data transfer object for a breed, containing its unique identifier and name.
/// </summary>
/// <param name="Id">The unique identifier of the breed.</param>
/// <param name="Name">The name of the breed.</param>
public sealed record BreedDto(
   Guid Id,
   string Name);
