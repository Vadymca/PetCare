namespace PetCare.Domain.ValueObjects;
using NetTopologySuite.Geometries;
using PetCare.Domain.Common;
using System.Device.Location;

/// <summary>
/// Represents geographical coordinates (latitude and longitude) as a value object.
/// </summary>
public sealed class Coordinates : ValueObject
{
    private const int Srid = 4326;

    private Coordinates(Point point) => this.Point = point;

    /// <summary>
    /// Gets the underlying Point (with SRID 4326).
    /// </summary>
    public Point Point { get; }

    /// <summary>
    /// Gets the latitude value.
    /// </summary>
    public double Latitude => this.Point.Y;

    /// <summary>
    /// Gets the longitude value.
    /// </summary>
    public double Longitude => this.Point.X;

    /// <summary>
    /// Gets a default origin coordinate (0, 0).
    /// </summary>
    public static Coordinates Origin => From(0, 0);

    /// <summary>
    /// Creates a new <see cref="Coordinates"/> instance from latitude and longitude.
    /// </summary>
    /// <param name="latitude">Latitude in decimal degrees.</param>
    /// <param name="longitude">Longitude in decimal degrees.</param>
    /// <returns>A new <see cref="Coordinates"/> object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when latitude or longitude is out of range.</exception>
    public static Coordinates From(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Широта повинна бути в межах від -90 до 90.");
        }

        if (longitude is < -180 or > 180)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Довгота повинна бути в межах від -180 до 180.");
        }

        var point = new Point(longitude, latitude) { SRID = Srid };
        return new Coordinates(point);
    }

    /// <summary>
    /// Calculates the distance between this coordinate and another in meters.
    /// </summary>
    /// <param name="other">The target <see cref="Coordinates"/> to calculate distance to.</param>
    /// <returns>The distance in meters.</returns>
    public double CalculateDistance(Coordinates other)
    {
        var current = new GeoCoordinate(this.Latitude, this.Longitude);
        var target = new GeoCoordinate(other.Latitude, other.Longitude);

        return current.GetDistanceTo(target);
    }

    /// <inheritdoc/>
    public override string ToString() => $"({this.Latitude}, {this.Longitude})";

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() =>
       new object[] { this.Latitude, this.Longitude };
}
