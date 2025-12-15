namespace PetCare.Application.Features.AdoptionApplications.GetApproved;

using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to retrieve all approved adoption applications.
/// </summary>
public sealed record GetApprovedAdoptionApplicationsCommand
    : IRequest<IReadOnlyList<AdoptionApplicationListDto>>;
