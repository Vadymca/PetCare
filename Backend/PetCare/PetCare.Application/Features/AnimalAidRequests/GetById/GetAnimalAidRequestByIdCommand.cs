namespace PetCare.Application.Features.AnimalAidRequests.GetById;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;

/// <summary>
/// Represents a request to retrieve a specific animal aid request by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the animal aid request.</param>
public sealed record GetAnimalAidRequestByIdCommand(Guid Id) : IRequest<AnimalAidRequestDetailsDto>;
