namespace PetCare.Application.Dtos.AnimalDtos;
using System;

public sealed record BreedDto(
   Guid Id,
   string Name);
