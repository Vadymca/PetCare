namespace PetCare.Application.Dtos.AdoptionApplicationDtos;

using PetCare.Domain.Enums;
using System;

public sealed record AdoptionApplicationDto(
 Guid Id,
 Guid UserId,
 Guid AnimalId,
 AdoptionStatus Status,
 DateTime ApplicationDate,
 string? Comment,
 string? AdminNotes,
 string? RejectionReason,
 DateTime CreatedAt,
 DateTime UpdatedAt,
 Guid? ApprovedBy);
