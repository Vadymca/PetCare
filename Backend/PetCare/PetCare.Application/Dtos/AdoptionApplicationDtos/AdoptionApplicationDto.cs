namespace PetCare.Application.Dtos.AdoptionApplicationDtos;

using System;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a data transfer object for an adoption application, containing information about the applicant, the
/// animal, application status, and related metadata.
/// </summary>
/// <param name="Id">The unique identifier of the adoption application.</param>
/// <param name="UserId">The unique identifier of the user who submitted the application.</param>
/// <param name="AnimalId">The unique identifier of the animal for which the adoption application was submitted.</param>
/// <param name="Status">The current status of the adoption application.</param>
/// <param name="ApplicationDate">The date and time when the adoption application was submitted.</param>
/// <param name="Comment">An optional comment provided by the applicant. May be null if no comment was supplied.</param>
/// <param name="AdminNotes">Optional notes added by an administrator regarding the application. May be null if no notes are present.</param>
/// <param name="RejectionReason">The reason for rejection if the application was denied. May be null if the application was not rejected.</param>
/// <param name="CreatedAt">The date and time when the application record was created.</param>
/// <param name="UpdatedAt">The date and time when the application record was last updated.</param>
/// <param name="ApprovedBy">The unique identifier of the administrator who approved the application, or null if the application has not been
/// approved.</param>
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
