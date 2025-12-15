namespace PetCare.Application.Features.AdoptionApplications.GetRejected;

using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to retrieve all rejected adoption applications.
/// </summary>
public sealed record GetRejectedAdoptionApplicationsCommand
    : IRequest<IReadOnlyList<AdoptionApplicationListDto>>;
