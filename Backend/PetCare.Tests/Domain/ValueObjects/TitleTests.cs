// <copyright file="TitleTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.ValueObjects;
using FluentAssertions;
using PetCare.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Title"/> value object.
/// </summary>
public class TitleTests
{
    /// <summary>
    /// Valid title string should create a Title instance successfully.
    /// </summary>
    [Fact]
    public void Create_ValidTitle_ShouldCreateInstance()
    {
        // Arrange
        const string validTitle = "This is a valid title";

        // Act
        Title title = Title.Create(validTitle);

        // Assert
        title.Should().NotBeNull();
        title.Value.Should().Be(validTitle);
        title.ToString().Should().Be(validTitle);
    }

    /// <summary>
    /// Null, empty or whitespace title should throw ArgumentException.
    /// </summary>
    /// <param name="invalidTitle">Invalid title string.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ShouldThrowArgumentException(string? invalidTitle)
    {
        // Act
        Action act = () => Title.Create(invalidTitle!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Заголовок не може бути порожнім.*")
            .And.ParamName.Should().Be("title");
    }

    /// <summary>
    /// Title exceeding maximum length should throw ArgumentException.
    /// </summary>
    [Fact]
    public void Create_TooLongTitle_ShouldThrowArgumentException()
    {
        // Arrange
        string tooLongTitle = new string('a', 256); // 256 chars, max allowed 255

        // Act
        Action act = () => Title.Create(tooLongTitle);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Назва не може бути довшою за 255 символів.*")
            .And.ParamName.Should().Be("title");
    }

    /// <summary>
    /// Two Title instances with the same value should be equal.
    /// </summary>
    [Fact]
    public void Equality_SameValue_ShouldBeEqual()
    {
        // Arrange
        const string value = "Equal Title";
        var title1 = Title.Create(value);
        var title2 = Title.Create(value);

        // Act & Assert
        title1.Should().Be(title2);
        title1.GetHashCode().Should().Be(title2.GetHashCode());
    }

    /// <summary>
    /// Two Title instances with different values should not be equal.
    /// </summary>
    [Fact]
    public void Equality_DifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        var title1 = Title.Create("Title One");
        var title2 = Title.Create("Title Two");

        // Act & Assert
        title1.Should().NotBe(title2);
    }
}
