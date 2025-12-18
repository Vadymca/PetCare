namespace PetCare.Application.Features.AnimalAidRequests.Create;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a command to create a new animal aid request.
/// </summary>
public sealed record CreateAnimalAidRequestCommand(
    Guid? ShelterId,
    string Title,
    string ShortDescription,
    string? Description,
    string Category,
    AidStatus Status,
    decimal? EstimatedCost,
    List<string>? Photos,
    string? CuratorFullName,
    string? ContactPhone,
    bool IsUrgent) : IRequest<AnimalAidRequestDetailsDto>;
