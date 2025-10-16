namespace PetCare.Api.Endpoints.Shelters;

/// <summary>
/// Represents the body of an update shelter request.
/// </summary>
public sealed record UpdateShelterBody(
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
 Dictionary<string, string>? SocialMedia);
