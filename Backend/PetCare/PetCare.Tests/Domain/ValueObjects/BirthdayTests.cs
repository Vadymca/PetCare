namespace PetCare.Tests.Domain.ValueObjects;
using PetCare.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Tests for the <see cref="Birthday"/> value object.
/// </summary>
public class BirthdayTests
{
    /// <summary>
    /// Validates that creating a Birthday with a valid date succeeds.
    /// </summary>
    [Fact]
    public void Create_ValidDate_ShouldReturnBirthday()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30));

        // Act
        var birthday = Birthday.Create(date);

        // Assert
        Assert.Equal(date, birthday.Value);
    }

    /// <summary>
    /// Validates that creating a Birthday with a future date throws exception.
    /// </summary>
    [Fact]
    public void Create_FutureDate_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Birthday.Create(futureDate));
    }

    /// <summary>
    /// Validates that the IsValid method correctly validates dates.
    /// </summary>
    [Fact]
    public void IsValid_ShouldReturnTrueForValidDates()
    {
        // Arrange
        var validDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-40));

        // Act
        var isValid = Birthday.IsValid(validDate);

        // Assert
        Assert.True(isValid);
    }
}
