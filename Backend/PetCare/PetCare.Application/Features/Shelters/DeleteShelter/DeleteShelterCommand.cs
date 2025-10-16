namespace PetCare.Application.Features.Shelters.DeleteShelter;

using System;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a request to delete a shelter identified by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the shelter to delete.</param>
public sealed record DeleteShelterCommand(Guid Id) : IRequest<DeleteShelterResponseDto>;
