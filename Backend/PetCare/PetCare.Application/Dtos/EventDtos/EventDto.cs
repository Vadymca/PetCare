namespace PetCare.Application.Dtos.EventDtos;

using System;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a data transfer object containing information about an event, including its identity, details,
/// scheduling, location, type, status, and audit timestamps.
/// </summary>
/// <param name="Id">The unique identifier of the event.</param>
/// <param name="ShelterId">The identifier of the associated shelter, if applicable; otherwise, <see langword="null"/>.</param>
/// <param name="Title">The title or name of the event.</param>
/// <param name="Description">A description providing additional details about the event, or <see langword="null"/> if not specified.</param>
/// <param name="EventDate">The scheduled date and time of the event, or <see langword="null"/> if not set.</param>
/// <param name="Location">The geographical coordinates of the event location, or <see langword="null"/> if not specified.</param>
/// <param name="Address">The address of the event location, or <see langword="null"/> if not provided.</param>
/// <param name="Type">The type or category of the event.</param>
/// <param name="Status">The current status of the event.</param>
/// <param name="CreatedAt">The date and time when the event was created, in UTC.</param>
/// <param name="UpdatedAt">The date and time when the event was last updated, in UTC.</param>
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
