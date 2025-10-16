namespace PetCare.Application.Features.Shelters.UpdateShelter;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a command to update the details of an existing shelter.
/// </summary>
/// <remarks>Only the fields provided with non-null values will be updated. Fields set to null will remain
/// unchanged.</remarks>
/// <param name="Id">The unique identifier of the shelter to update.</param>
/// <param name="Name">The new name of the shelter, or null to leave unchanged.</param>
/// <param name="Address">The new address of the shelter, or null to leave unchanged.</param>
/// <param name="Latitude">The new latitude coordinate of the shelter location, or null to leave unchanged.</param>
/// <param name="Longitude">The new longitude coordinate of the shelter location, or null to leave unchanged.</param>
/// <param name="ContactPhone">The new contact phone number for the shelter, or null to leave unchanged.</param>
/// <param name="ContactEmail">The new contact email address for the shelter, or null to leave unchanged.</param>
/// <param name="Description">The new description of the shelter, or null to leave unchanged.</param>
/// <param name="Capacity">The new maximum capacity of the shelter, or null to leave unchanged.</param>
/// <param name="Photos">A list of new photo URLs for the shelter, or null to leave unchanged.</param>
/// <param name="VirtualTourUrl">The new virtual tour URL for the shelter, or null to leave unchanged.</param>
/// <param name="WorkingHours">The new working hours for the shelter, or null to leave unchanged.</param>
/// <param name="SocialMedia">A dictionary containing updated social media links for the shelter, or null to leave unchanged.</param>
public sealed record UpdateShelterCommand(
 Guid Id,
 string? Name,
 string? Address,
 double? Latitude,
 double? Longitude,
 string? ContactPhone,
 string? ContactEmail,
 string? Description,
 int? Capacity,
 List<string>? Photos,
 string? VirtualTourUrl,
 string? WorkingHours,
 Dictionary<string, string>? SocialMedia) : IRequest<ShelterDto>;
