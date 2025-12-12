namespace PetCare.Application.Features.AnimalAidRequests.UpdateStatus;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Domain.Enums;

/// <summary>
/// Command to update the status of an AnimalAidRequest.
/// </summary>
public sealed record UpdateAnimalAidRequestStatusCommand(
    Guid Id,
    AidStatus Status) : IRequest<AnimalAidRequestDetailsDto>;
