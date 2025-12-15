namespace PetCare.Application.Features.AdoptionApplications.Update;

using System;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to update an existing adoption application.
/// </summary>
/// <param name="Id">The identifier of the adoption application.</param>
/// <param name="Comment">Updated user comment.</param>
/// <param name="AdminNotes">Updated administrative notes.</param>
public sealed record UpdateAdoptionApplicationCommand(
    Guid Id,
    string? Comment,
    string? AdminNotes) : IRequest<AdoptionApplicationDetailsDto>;
