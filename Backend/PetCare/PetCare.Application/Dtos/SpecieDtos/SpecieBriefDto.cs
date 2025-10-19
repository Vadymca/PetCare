namespace PetCare.Application.Dtos.SpecieDtos;

using System;

/// <summary>
/// Represents a brief view of a species with its identifier and name.
/// </summary>
/// <param name="Id">The unique identifier of the species.</param>
/// <param name="Name">The name of the species.</param>
public sealed record SpecieBriefDto(Guid Id, string Name);
