namespace PetCare.Application.Dtos.AdoptionApplicationDtos;

using System;
using PetCare.Domain.Enums;

/// <summary>
/// Data transfer object used to display detailed information about an adoption application.
/// </summary>
public sealed record AdoptionApplicationDetailsDto(
    Guid Id,
    Guid UserId,
    Guid AnimalId,
    AdoptionStatus Status,
    DateTime ApplicationDate,
    DateTime? MeetingDate,
    DateTime? AdoptionDate,
    DateTime? RejectionDate,
    string Comment,
    string AdminNotes,
    string RejectionReason,
    string? CuratorName,
    string? CuratorPhone,
    DateTime CreatedAt,
    DateTime UpdatedAt);
