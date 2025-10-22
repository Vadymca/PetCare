namespace PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a geographic coordinate with latitude and longitude values.
/// </summary>
/// <param name="Lat">The latitude component of the coordinate, in decimal degrees. Positive values indicate locations north of the
/// equator; negative values indicate locations south of the equator.</param>
/// <param name="Lng">The longitude component of the coordinate, in decimal degrees. Positive values indicate locations east of the prime
/// meridian; negative values indicate locations west of the prime meridian.</param>
public sealed record CoordinatesDto(double Lat, double Lng);
