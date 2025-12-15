namespace PetCare.Application.Features.AdoptionApplications.ChangeStatus;

using System;
using MediatR;
using PetCare.Domain.Enums;

/// <summary>
/// Command to change the status of an adoption application.
/// </summary>
/// <param name="Id">The identifier of the adoption application.</param>
/// <param name="Status">The new status to apply.</param>
/// <param name="AdminId">The identifier of the administrator performing the action.</param>
/// <param name="RejectionReason">Optional rejection reason.</param>
public sealed record ChangeAdoptionApplicationStatusCommand(
    Guid Id,
    AdoptionStatus Status,
    Guid AdminId,
    string? RejectionReason,
    string? CuratorName = null,
    string? CuratorPhone = null) : IRequest;
