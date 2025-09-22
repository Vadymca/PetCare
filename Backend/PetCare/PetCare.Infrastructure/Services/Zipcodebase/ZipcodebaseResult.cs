namespace PetCare.Infrastructure.Services.Zipcodebase;

/// <summary>
/// Represents a single location entry in Zipcodebase API response.
/// </summary>
public sealed record ZipcodebaseResult(
    string Postal_Code,
    string City,
    string State,
    string Country_Code);
