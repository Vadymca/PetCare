namespace PetCare.Application.Dtos.AnimalDtos;

using System;

/// <summary>
/// Represents summary information about an animal shelter, including its unique identifier and name.
/// </summary>
/// <param name="Id">The unique identifier of the shelter.</param>
/// <param name="Name">The name of the shelter.</param>
public sealed record ShelterInfoDto(
   Guid Id,
   string Name,
   string Slug);
