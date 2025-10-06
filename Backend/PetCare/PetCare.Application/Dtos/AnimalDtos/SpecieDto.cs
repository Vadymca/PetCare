namespace PetCare.Application.Dtos.AnimalDtos;
using System;

public sealed record SpecieDto(
     Guid Id,
     string Name);
