namespace PetCare.Application.Features.AdoptionApplications.GetAll;

using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to retrieve all adoption applications.
/// </summary>
public sealed record GetAdoptionApplicationsCommand
    : IRequest<IReadOnlyList<AdoptionApplicationDetailsDto>>;