namespace PetCare.Application.Features.AdoptionApplications.ChangeStatus;

using System;
using MediatR;
using PetCare.Domain.Enums;

/// <summary>
/// Command to change the status of an adoption application.
/// </summary>
public sealed record ChangeAdoptionApplicationStatusCommand(
    Guid Id,
    AdoptionStatus Status,
    Guid AdminId,
    string? RejectionReason,
    string? CuratorName = null,
    string? CuratorPhone = null,
    DateTime? MeetingDate = null) : IRequest;
