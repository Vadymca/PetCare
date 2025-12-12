namespace PetCare.Application.Features.AnimalAidRequests.GetAllAnimalAidRequests;

using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;

/// <summary>
/// Represents a request to retrieve all animal aid requests.
/// </summary>
public sealed record GetAllAnimalAidRequestsCommand() : IRequest<IReadOnlyList<AnimalAidRequestListDto>>;
