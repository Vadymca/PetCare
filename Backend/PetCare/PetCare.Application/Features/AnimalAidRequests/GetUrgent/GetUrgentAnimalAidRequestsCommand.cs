namespace PetCare.Application.Features.AnimalAidRequests.GetUrgent;

using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;

/// <summary>
/// Query for retrieving urgent animal aid requests with full donation information.
/// </summary>
public sealed record GetUrgentAnimalAidRequestsCommand
    : IRequest<List<UrgentAnimalAidRequestDto>>;
