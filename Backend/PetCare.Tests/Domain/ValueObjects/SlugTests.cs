// <copyright file="SlugTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.ValueObjects;
using FluentAssertions;
using PetCare.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Slug"/> value object.
/// </summary>
public class SlugTests
{
    /// <summary>
    /// Tests creation of slug from valid Ukrainian and Latin input strings.
    /// </summary>
    /// <param name="input">The input string to convert to slug.</param>
    /// <param name="expected">The expected slug result.</param>
    [Theory]
    [InlineData("Барсік", "barsik")]
    [InlineData("Київ", "kyiv")]
    [InlineData("Щастя", "shchastia")]
    [InlineData("Тестовий слаг", "testovyi-slah")]
    [InlineData("Привіт, світе!", "pryvit-svite")]
    [InlineData("    Пробіли   і_підкреслення ", "probily-i-pidkreslennia")]
    [InlineData("123 числа", "123-chysla")]
    [InlineData("slug-with-dashes", "slug-with-dashes")]
    public void Create_ValidUkrainianAndLatinStrings_ShouldCreateExpectedSlug(string input, string expected)
    {
        // Act
        Slug slug = Slug.Create(input);

        // Assert
        slug.Value.Should().Be(expected);
        slug.ToString().Should().Be(expected);
    }

    /// <summary>
    /// Tests that creating slug with empty or whitespace input throws <see cref="ArgumentException"/>.
    /// </summary>
    [Fact]
    public void Create_EmptyOrWhitespace_ShouldThrowArgumentException()
    {
        // Arrange
        string[] invalidInputs = { null!, string.Empty, "   " };

        foreach (var input in invalidInputs)
        {
            // Act
            Action act = () => Slug.Create(input!);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Вхідне значення не може бути порожнім.*")
                .And.ParamName.Should().Be("name");
        }
    }

    /// <summary>
    /// Tests that creating slug with invalid characters throws <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="input">The invalid input string.</param>
    [Theory]
    [InlineData("!!!")]
    [InlineData("!!!@@@###")]
    [InlineData("---")]
    public void Create_InvalidAfterNormalization_ShouldThrowArgumentException(string input)
    {
        // Act
        Action act = () => Slug.Create(input);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Slug не дійсний.*");
    }

    /// <summary>
    /// Tests the <see cref="Slug.IsValid(string)"/> method with valid and invalid inputs.
    /// </summary>
    [Fact]
    public void IsValid_ValidAndInvalidCases()
    {
        Slug.IsValid("valid-slug-123").Should().BeTrue();
        Slug.IsValid("invalid slug!").Should().BeFalse();
        Slug.IsValid(string.Empty).Should().BeFalse();
        Slug.IsValid(null!).Should().BeFalse();
    }
}
