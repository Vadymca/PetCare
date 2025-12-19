namespace PetCare.Application.Features.AdoptionApplications.Update;

using System;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to update editable details of an existing adoption application.
/// </summary>
/// <param name="Id">The identifier of the adoption application.</param>
/// <param name="Comment">Updated user comment.</param>
/// <param name="AdminNotes">Updated administrative notes.</param>
/// <param name="CuratorName">Updated curator name.</param>
/// <param name="CuratorPhone">Updated curator phone number.</param>
/// <param name="MeetingDate">Updated meeting date.</param>
public sealed record UpdateAdoptionApplicationCommand(
    Guid Id,
    string? Comment,
    string? AdminNotes,
    string? CuratorName,
    string? CuratorPhone,
    DateTime? MeetingDate)
    : IRequest<AdoptionApplicationDetailsDto>;