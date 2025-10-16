namespace PetCare.Application.Dtos.ShelterDtos;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a lightweight DTO for displaying a list of shelters (without description).
/// </summary>
/// <param name="Id">The unique identifier of the shelter.</param>
/// <param name="Name">The name of the shelter.</param>
/// <param name="Slug">The slug of the shelter.</param>
/// <param name="City">The city where the shelter is located (derived from Address).</param>
/// <param name="Address">The full address of the shelter.</param>
/// <param name="ContactPhone">The contact phone number.</param>
/// <param name="ContactEmail">The contact email address.</param>
/// <param name="Capacity">The maximum capacity of the shelter.</param>
/// <param name="CurrentOccupancy">The current number of animals in the shelter.</param>
/// <param name="HasFreeCapacity">Indicates whether the shelter has available capacity.</param>
/// <param name="VirtualTourUrl">The virtual tour URL, if available.</param>
/// <param name="WorkingHours">The working hours of the shelter.</param>
/// <param name="Photos">The collection of photo URLs for the shelter.</param>
/// <param name="SocialMedia">The collection of social media links for the shelter.</param>
/// <param name="CreatedAt">The creation date of the shelter.</param>
public sealed record ShelterListDto(
    Guid Id,
    string Name,
    string Slug,
    string? Address,
    string? ContactPhone,
    string? ContactEmail,
    int Capacity,
    int CurrentOccupancy,
    bool HasFreeCapacity,
    string? VirtualTourUrl,
    string? WorkingHours,
    IReadOnlyList<string> Photos,
    IReadOnlyDictionary<string, string> SocialMedia,
    DateTime CreatedAt);
