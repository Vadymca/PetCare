namespace PetCare.Application.Dtos.ShelterDtos;
using System;

public sealed record ShelterDto(
 Guid Id,
 string Slug,
 string Name,
 string Address,
 string? ContactPhone,
 string? ContactEmail,
 string? Description,
 int Capacity,
 int CurrentOccupancy,
 string? VirtualTourUrl,
 string? WorkingHours);
