namespace PetCare.Tests.Domain.ValueObjects;
using FluentAssertions;
using PetCare.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Unit tests for <see cref="Coordinates"/> value object.
/// </summary>
public class CoordinatesTests
{
    /// <summary>
    /// Valid latitude and longitude should create instance.
    /// </summary>
    /// <param name="latitude">Latitude in decimal degrees.</param>
    /// <param name="longitude">Longitude in decimal degrees.</param>
    [Theory]
    [InlineData(0, 0)]
    [InlineData(45.0, 90.0)]
    [InlineData(-90, -180)]
    [InlineData(90, 180)]
    public void From_ValidCoordinates_ShouldCreateInstance(double latitude, double longitude)
    {
        // Act
        Coordinates coords = Coordinates.From(latitude, longitude);

        // Assert
        coords.Should().NotBeNull();
        coords.Latitude.Should().Be(latitude);
        coords.Longitude.Should().Be(longitude);
        coords.Point.SRID.Should().Be(4326);
    }

    /// <summary>
    /// Latitude out of range should throw.
    /// </summary>
    /// <param name="latitude">Invalid latitude value outside the range -90 to 90.</param>
    [Theory]
    [InlineData(-90.1)]
    [InlineData(90.1)]
    public void From_InvalidLatitude_ShouldThrowArgumentOutOfRangeException(double latitude)
    {
        // Arrange
        double longitude = 0;

        // Act
        Action act = () => Coordinates.From(latitude, longitude);

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Широта повинна бути в межах від -90 до 90.*")
            .And.ParamName.Should().Be("latitude");
    }

    /// <summary>
    /// Longitude out of range should throw.
    /// </summary>
    /// /// <param name="longitude">Longitude in decimal degrees.</param>
    [Theory]
    [InlineData(-180.1)]
    [InlineData(180.1)]
    public void From_InvalidLongitude_ShouldThrowArgumentOutOfRangeException(double longitude)
    {
        // Arrange
        double latitude = 0;

        // Act
        Action act = () => Coordinates.From(latitude, longitude);

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Довгота повинна бути в межах від -180 до 180.*")
            .And.ParamName.Should().Be("longitude");
    }

    /// <summary>
    /// CalculateDistance returns expected value.
    /// </summary>
    [Fact]
    public void CalculateDistance_ShouldReturnCorrectValue()
    {
        // Arrange
        Coordinates coord1 = Coordinates.From(50.4501, 30.5234); // Kyiv
        Coordinates coord2 = Coordinates.From(48.3794, 31.1656); // Ukraine center approx

        // Act
        double distance = coord1.CalculateDistance(coord2);

        // Assert
        // Distance should be positive and reasonable (about 230 km)
        distance.Should().BeGreaterThan(200_000);
        distance.Should().BeLessThan(300_000);
    }

    /// <summary>
    /// Equality of two instances with same values.
    /// </summary>
    [Fact]
    public void Equality_SameCoordinates_ShouldBeEqual()
    {
        // Arrange
        Coordinates coords1 = Coordinates.From(10, 20);
        Coordinates coords2 = Coordinates.From(10, 20);

        // Act & Assert
        coords1.Should().Be(coords2);
        coords1.GetHashCode().Should().Be(coords2.GetHashCode());
    }

    /// <summary>
    /// Inequality of two instances with different values.
    /// </summary>
    [Fact]
    public void Equality_DifferentCoordinates_ShouldNotBeEqual()
    {
        // Arrange
        Coordinates coords1 = Coordinates.From(10, 20);
        Coordinates coords2 = Coordinates.From(10, 21);

        // Act & Assert
        coords1.Should().NotBe(coords2);
    }
}
