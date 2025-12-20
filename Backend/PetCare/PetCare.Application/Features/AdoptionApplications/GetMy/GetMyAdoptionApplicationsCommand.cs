namespace PetCare.Application.Features.AdoptionApplications.GetMy;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to retrieve adoption applications created by the current user.
/// </summary>
/// <param name="UserId">The identifier of the current user.</param>
public sealed record GetMyAdoptionApplicationsCommand(
    Guid UserId)
    : IRequest<IReadOnlyList<AdoptionApplicationDetailsDto>>;
