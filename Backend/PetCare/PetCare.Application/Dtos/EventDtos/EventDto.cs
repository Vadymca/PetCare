namespace PetCare.Application.Dtos.EventDtos;

using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using System;

public sealed record EventDto(
 Guid Id,
 Guid? ShelterId,
 string Title,
 string? Description,
 DateTime? EventDate,
 Coordinates? Location,
 string? Address,
 EventType Type,
 EventStatus Status,
 DateTime CreatedAt,
 DateTime UpdatedAt);
