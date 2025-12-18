namespace PetCare.Application.Features.AdoptionApplications.Approve;

using System;
using MediatR;

/// <summary>
/// Command to approve an adoption application.
/// </summary>
public sealed record ApproveAdoptionApplicationCommand(
    Guid Id,
    Guid AdminId,
    string? CuratorName = null,
    string? CuratorPhone = null,
    DateTime? MeetingDate = null) : IRequest<Unit>;
