// <copyright file="PhoneNumberTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.ValueObjects;
using FluentAssertions;
using PetCare.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="PhoneNumber"/> value object.
/// </summary>
public class PhoneNumberTests
{
    /// <summary>
    /// Creating a <see cref="PhoneNumber"/> with valid E.164 format should succeed.
    /// </summary>
    /// <param name="input">Valid phone number string in E.164 format.</param>
    [Theory]
    [InlineData("+380501112233")]
    [InlineData("+14155552671")]
    [InlineData("+447911123456")]
    public void Create_WithValidPhone_ShouldCreateInstance(string input)
    {
        // Act
        PhoneNumber phone = PhoneNumber.Create(input);

        // Assert
        phone.Should().NotBeNull();
        phone.Value.Should().Be(input.Trim());
        phone.ToString().Should().Be(input.Trim());
        phone.Format().Should().Be(input.Trim());
    }

    /// <summary>
    /// Creating a <see cref="PhoneNumber"/> with invalid input should throw an <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="input">Invalid phone number string.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123456")]
    [InlineData("+")]
    [InlineData("+123abc456")]
    [InlineData("++380501112233")]
    public void Create_WithInvalidPhone_ShouldThrowArgumentException(string input)
    {
        // Act
        Action act = () => PhoneNumber.Create(input);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Номер телефону повинен бути у дійсному форматі E.164*")
            .And.ParamName.Should().Be("phone");
    }

    /// <summary>
    /// Two <see cref="PhoneNumber"/> instances with same values should be equal.
    /// </summary>
    [Fact]
    public void Equality_SameValue_ShouldBeEqual()
    {
        // Arrange
        string input = "+380501112233";
        PhoneNumber phone1 = PhoneNumber.Create(input);
        PhoneNumber phone2 = PhoneNumber.Create(input);

        // Act & Assert
        phone1.Should().Be(phone2);
        phone1.GetHashCode().Should().Be(phone2.GetHashCode());
    }

    /// <summary>
    /// Two <see cref="PhoneNumber"/> instances with different values should not be equal.
    /// </summary>
    [Fact]
    public void Equality_DifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        PhoneNumber phone1 = PhoneNumber.Create("+380501112233");
        PhoneNumber phone2 = PhoneNumber.Create("+14155552671");

        // Act & Assert
        phone1.Should().NotBe(phone2);
    }

    /// <summary>
    /// IsValid should return true for valid phone numbers and false otherwise.
    /// </summary>
    /// <param name="input">Phone number string.</param>
    /// <param name="expected">Expected validation result.</param>
    [Theory]
    [InlineData("+380501112233", true)]
    [InlineData("+1234567", true)]
    [InlineData("123", false)]
    [InlineData("++380", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValid_ShouldReturnExpectedResult(string? input, bool expected)
    {
        // Act
        bool result = PhoneNumber.IsValid(input ?? string.Empty);

        // Assert
        result.Should().Be(expected);
    }
}
