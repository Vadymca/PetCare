namespace PetCare.Application.Features.AdoptionApplications.GetById;

using System;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to retrieve an adoption application by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the adoption application.</param>
public sealed record GetAdoptionApplicationByIdCommand(Guid Id)
    : IRequest<AdoptionApplicationDetailsDto>;
