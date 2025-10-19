namespace PetCare.Application.Dtos.SpecieDtos;

using System;

/// <summary>
/// Represents a breed in the system.
/// </summary>
public sealed record BreedListDto(
    Guid Id,
    string Name,
    string? Description);
