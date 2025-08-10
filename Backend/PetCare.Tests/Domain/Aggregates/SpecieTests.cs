namespace PetCare.Tests.Domain.Aggregates;
using FluentAssertions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using System;
using Xunit;

/// <summary>
/// Unit tests for <see cref="Specie"/> aggregate.
/// </summary>
public class SpecieTests
{
    /// <summary>
    /// Tests that <see cref="Specie.Create(string)"/> creates instance with correct name.
    /// </summary>
    [Fact]
    public void Create_ShouldCreateSpecie_WhenNameIsValid()
    {
        // Arrange
        const string validName = "Cat";

        // Act
        var specie = Specie.Create(validName);

        // Assert
        specie.Should().NotBeNull();
        specie.Name.Value.Should().Be(validName);
        specie.Breeds.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that <see cref="Specie.Create(string)"/> throws if name is invalid.
    /// </summary>
    /// <param name="invalidName">The invalid name value to test (null, empty, or whitespace).</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldThrowArgumentException_WhenNameIsInvalid(string? invalidName)
    {
        // Act
        Action act = () => Specie.Create(invalidName!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*");
    }

    /// <summary>
    /// Tests that <see cref="Specie.Rename(string)"/> changes the name.
    /// </summary>
    [Fact]
    public void Rename_ShouldChangeName_WhenNewNameIsValid()
    {
        // Arrange
        var specie = Specie.Create("Dog");
        const string newName = "Wolf";

        // Act
        specie.Rename(newName);

        // Assert
        specie.Name.Value.Should().Be(newName);
    }

    /// <summary>
    /// Tests that <see cref="Specie.Rename(string)"/> throws if new name is invalid.
    /// </summary>
    /// <param name="invalidName">The invalid new name value to test (null, empty, or whitespace).</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Rename_ShouldThrowArgumentException_WhenNewNameIsInvalid(string? invalidName)
    {
        // Arrange
        var specie = Specie.Create("Dog");

        // Act
        Action act = () => specie.Rename(invalidName!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*");
    }

    /// <summary>
    /// Tests that <see cref="Specie.AddBreed(Breed)"/> adds a breed.
    /// </summary>
    [Fact]
    public void AddBreed_ShouldAddBreed_WhenBreedIsValidAndNotExists()
    {
        // Arrange
        var specie = Specie.Create("Bird");
        var breed = Breed.Create(Guid.NewGuid(), "Parrot", null);

        // Act
        specie.AddBreed(breed);

        // Assert
        specie.Breeds.Should().ContainSingle(b => b.Id == breed.Id);
    }

    /// <summary>
    /// Tests that <see cref="Specie.AddBreed(Breed)"/> throws when breed is null.
    /// </summary>
    [Fact]
    public void AddBreed_ShouldThrowArgumentNullException_WhenBreedIsNull()
    {
        // Arrange
        var specie = Specie.Create("Fish");

        // Act
        Action act = () => specie.AddBreed(null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*Порода не може бути null.*");
    }

    /// <summary>
    /// Tests that <see cref="Specie.AddBreed(Breed)"/> throws when breed already exists.
    /// </summary>
    [Fact]
    public void AddBreed_ShouldThrowInvalidOperationException_WhenBreedAlreadyExists()
    {
        // Arrange
        var specie = Specie.Create("Reptile");
        var breed = Breed.Create(Guid.NewGuid(), "Lizard", null);
        specie.AddBreed(breed);

        // Act
        Action act = () => specie.AddBreed(breed);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Ця порода вже додана до цього виду.");
    }

    /// <summary>
    /// Tests that <see cref="Specie.RemoveBreed(Guid)"/> removes existing breed.
    /// </summary>
    [Fact]
    public void RemoveBreed_ShouldRemoveBreed_WhenBreedExists()
    {
        // Arrange
        var specie = Specie.Create("Rodent");
        var breed = Breed.Create(Guid.NewGuid(), "Hamster", null);
        specie.AddBreed(breed);

        // Act
        var result = specie.RemoveBreed(breed.Id);

        // Assert
        result.Should().BeTrue();
        specie.Breeds.Should().NotContain(b => b.Id == breed.Id);
    }

    /// <summary>
    /// Tests that <see cref="Specie.RemoveBreed(Guid)"/> returns false when breed not found.
    /// </summary>
    [Fact]
    public void RemoveBreed_ShouldReturnFalse_WhenBreedDoesNotExist()
    {
        // Arrange
        var specie = Specie.Create("Amphibian");
        var nonExistentBreedId = Guid.NewGuid();

        // Act
        var result = specie.RemoveBreed(nonExistentBreedId);

        // Assert
        result.Should().BeFalse();
    }
}
