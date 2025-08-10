namespace PetCare.Tests.Domain.ValueObjects;
using FluentAssertions;
using PetCare.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Name"/> value object.
/// </summary>
public class NameTests
{
    /// <summary>
    /// Creating a <see cref="Name"/> with valid input should succeed.
    /// </summary>
    [Fact]
    public void Create_WithValidName_ShouldCreateInstance()
    {
        // Arrange
        const string input = "Барсик";

        // Act
        Name name = Name.Create(input);

        // Assert
        name.Should().NotBeNull();
        name.Value.Should().Be(input);
        name.ToString().Should().Be(input);
    }

    /// <summary>
    /// Creating a <see cref="Name"/> with null or whitespace should throw <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="input">Invalid name input.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespace_ShouldThrowArgumentException(string? input)
    {
        // Act
        Action act = () => Name.Create(input!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Ім'я не може бути порожнім.*")
            .And.ParamName.Should().Be("value");
    }

    /// <summary>
    /// Two <see cref="Name"/> objects with the same value should be equal.
    /// </summary>
    [Fact]
    public void Equality_SameValue_ShouldBeEqual()
    {
        // Arrange
        const string input = "Мурчик";

        Name name1 = Name.Create(input);
        Name name2 = Name.Create(input);

        // Act & Assert
        name1.Should().Be(name2);
        name1.GetHashCode().Should().Be(name2.GetHashCode());
    }

    /// <summary>
    /// Two <see cref="Name"/> objects with different values should not be equal.
    /// </summary>
    [Fact]
    public void Equality_DifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        Name name1 = Name.Create("Барсик");
        Name name2 = Name.Create("Мурчик");

        // Act & Assert
        name1.Should().NotBe(name2);
    }
}
