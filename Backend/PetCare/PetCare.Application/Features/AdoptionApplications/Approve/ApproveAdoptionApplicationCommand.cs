namespace PetCare.Application.Features.AdoptionApplications.Approve;

using System;
using MediatR;

/// <summary>
/// Command to approve an adoption application.
/// </summary>
/// <param name="Id">The identifier of the adoption application to approve.</param>
/// <param name="AdminId">The identifier of the admin approving the application.</param>
public sealed record ApproveAdoptionApplicationCommand(
    Guid Id,
    Guid AdminId,
    string? CuratorName = null,
    string? CuratorPhone = null) : IRequest<Unit>;
