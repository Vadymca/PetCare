namespace PetCare.Application.Dtos.AdoptionApplicationDtos;

using System;
using PetCare.Domain.Enums;

/// <summary>
/// Data transfer object used to display an adoption application in a list.
/// </summary>
public sealed record AdoptionApplicationListDto(
    Guid Id,
    Guid UserId,
    Guid AnimalId,
    AdoptionStatus Status,
    DateTime ApplicationDate,
    string Comment,
    string AdminNotes);
