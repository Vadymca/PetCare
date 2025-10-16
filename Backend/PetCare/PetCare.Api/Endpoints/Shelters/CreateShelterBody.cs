namespace PetCare.Api.Endpoints.Shelters;

using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents the body of a create shelter request.
/// </summary>
public sealed record CreateShelterBody(
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    string ContactPhone,
    string ContactEmail,
    string? Description,
    int Capacity,
    int CurrentOccupancy,
    List<string>? Photos,
    string? VirtualTourUrl,
    string? WorkingHours,
    Dictionary<string, string>? SocialMedia,
    Guid? ManagerId);
