// <copyright file="MicrochipIdTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.ValueObjects;
using FluentAssertions;
using PetCare.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="MicrochipId"/> value object.
/// </summary>
public class MicrochipIdTests
{
    /// <summary>
    /// Valid microchip ID should create instance and normalize to uppercase.
    /// </summary>
    /// <param name="input">Valid microchip string.</param>
    [Theory]
    [InlineData("abcde")]
    [InlineData("12345")]
    [InlineData("a1b2c3")]
    [InlineData("AB12C34DE")]
    [InlineData("abcdE12345")]
    public void Create_WithValidMicrochipId_ShouldCreateInstance(string input)
    {
        // Act
        MicrochipId id = MicrochipId.Create(input);

        // Assert
        id.Should().NotBeNull();
        id.Value.Should().Be(input.Trim().ToUpperInvariant());
        id.ToString().Should().Be(id.Value);
    }

    /// <summary>
    /// Null or whitespace input should throw <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="input">Null or whitespace input.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespace_ShouldThrowArgumentException(string? input)
    {
        // Act
        Action act = () => MicrochipId.Create(input!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Ідентифікатор мікрочіпа не може бути порожнім.*")
            .And.ParamName.Should().Be("value");
    }

    /// <summary>
    /// Invalid format should throw <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="input">Input with invalid format.</param>
    [Theory]
    [InlineData("ab")] // Too short
    [InlineData("ab@cd123")] // Invalid symbol
    [InlineData("toolongtoolongtoolongtoolong")] // Too long
    [InlineData("абвгд123")] // Cyrillic letters
    public void Create_WithInvalidFormat_ShouldThrowArgumentException(string input)
    {
        // Act
        Action act = () => MicrochipId.Create(input);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Неправильний формат ідентифікатора мікрочіпа.*")
            .And.ParamName.Should().Be("value");
    }

    /// <summary>
    /// Two equal microchip IDs should compare equal.
    /// </summary>
    [Fact]
    public void Equality_SameValue_ShouldBeEqual()
    {
        // Arrange
        MicrochipId id1 = MicrochipId.Create("A1B2C3");
        MicrochipId id2 = MicrochipId.Create("a1b2c3");

        // Act & Assert
        id1.Should().Be(id2);
        id1.GetHashCode().Should().Be(id2.GetHashCode());
    }

    /// <summary>
    /// Two different microchip IDs should not compare equal.
    /// </summary>
    [Fact]
    public void Equality_DifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        MicrochipId id1 = MicrochipId.Create("A1B2C3");
        MicrochipId id2 = MicrochipId.Create("D4E5F6");

        // Act & Assert
        id1.Should().NotBe(id2);
    }

    /// <summary>
    /// IsValid should return true for valid input.
    /// </summary>
    /// <param name="input">Valid microchip string.</param>
    [Theory]
    [InlineData("ABC123")]
    [InlineData("xyz987")]
    [InlineData("A1B2C3")]
    public void IsValid_WithValidInput_ShouldReturnTrue(string input)
    {
        // Act
        bool result = MicrochipId.IsValid(input);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// IsValid should return false for invalid input.
    /// </summary>
    /// <param name="input">Invalid microchip string.</param>
    [Theory]
    [InlineData("")]
    [InlineData("12")]
    [InlineData("a@bc1")]
    [InlineData("абвгд123")]
    [InlineData(null)]
    public void IsValid_WithInvalidInput_ShouldReturnFalse(string? input)
    {
        // Act
        bool result = MicrochipId.IsValid(input);

        // Assert
        result.Should().BeFalse();
    }
}
