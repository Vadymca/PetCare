namespace PetCare.Application.Features.AnimalAidRequests.Update;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Domain.Enums;

/// <summary>
/// Command for updating an existing animal aid request.
/// Only non-null properties will be updated.
/// </summary>
/// <param name="Id">The unique identifier of the aid request.</param>
/// <param name="Title">New title, or null to leave unchanged.</param>
/// <param name="ShortDescription">New short description, or null to leave unchanged.</param>
/// <param name="Description">New description, or null to leave unchanged.</param>
/// <param name="Category">New category, or null to leave unchanged.</param>
/// <param name="EstimatedCost">New estimated cost, or null to leave unchanged.</param>
/// <param name="Photos">New list of photos, or null to leave unchanged.</param>
/// <param name="Status">New status, or null to leave unchanged.</param>
/// <param name="ContactPhone">New contact phone, or null to leave unchanged.</param>
/// <param name="IsUrgent">New urgency flag, or null to leave unchanged.</param>
public sealed record UpdateAnimalAidRequestCommand(
    Guid Id,
    string? Title,
    string? ShortDescription,
    string? Description,
    string? Category,
    decimal? EstimatedCost,
    List<string>? Photos,
    AidStatus? Status,
    string? CuratorFullName,
    string? ContactPhone,
    bool? IsUrgent) : IRequest<AnimalAidRequestDetailsDto>;
