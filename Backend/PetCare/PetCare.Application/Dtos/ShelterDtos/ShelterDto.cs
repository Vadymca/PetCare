namespace PetCare.Application.Dtos.ShelterDtos;

using System;

/// <summary>
/// Represents a data transfer object containing detailed information about a shelter, including identification, contact
/// details, capacity, and descriptive metadata.
/// </summary>
/// <param name="Id">The unique identifier of the shelter.</param>
/// <param name="Slug">The URL-friendly slug used to identify the shelter in web addresses. Cannot be null.</param>
/// <param name="Name">The name of the shelter. Cannot be null.</param>
/// <param name="Address">The physical address of the shelter. Cannot be null.</param>
/// <param name="ContactPhone">The contact phone number for the shelter, or null if not available.</param>
/// <param name="ContactEmail">The contact email address for the shelter, or null if not available.</param>
/// <param name="Description">A description of the shelter, or null if not provided.</param>
/// <param name="Capacity">The maximum number of occupants the shelter can accommodate.</param>
/// <param name="CurrentOccupancy">The current number of occupants in the shelter.</param>
/// <param name="VirtualTourUrl">A URL to a virtual tour of the shelter, or null if not available.</param>
/// <param name="WorkingHours">The working hours of the shelter, or null if not specified.</param>
public sealed record ShelterDto(
 Guid Id,
 string Slug,
 string Name,
 string Address,
 string? ContactPhone,
 string? ContactEmail,
 string? Description,
 int Capacity,
 int CurrentOccupancy,
 bool HasFreeCapacity,
 string? VirtualTourUrl,
 string? WorkingHours,
 IReadOnlyList<string> Photos,
 IReadOnlyDictionary<string, string> SocialMedia,
 DateTime CreatedAt,
 CoordinatesDto Coordinates);
