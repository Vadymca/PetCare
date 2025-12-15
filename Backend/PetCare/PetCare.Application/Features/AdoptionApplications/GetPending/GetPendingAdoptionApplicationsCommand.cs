namespace PetCare.Application.Features.AdoptionApplications.GetPending;

using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to retrieve all pending adoption applications.
/// </summary>
public sealed record GetPendingAdoptionApplicationsCommand
    : IRequest<IReadOnlyList<AdoptionApplicationListDto>>;
