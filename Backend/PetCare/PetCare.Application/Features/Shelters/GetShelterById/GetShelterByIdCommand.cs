namespace PetCare.Application.Features.Shelters.GetShelterById;

using System;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a request to retrieve a shelter by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the shelter.</param>
public sealed record GetShelterByIdCommand(Guid Id) : IRequest<ShelterDto?>;
