namespace PetCare.Tests.Domain.Specifications;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Events;
using PetCare.Domain.Specifications.Specie;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Unit tests for the Specie aggregate and SpecieByNameSpecification.
/// </summary>
public class SpecieTests
{
    /// <summary>
    /// Tests that creating a specie with a valid name succeeds.
    /// </summary>
    [Fact]
    public void Create_ShouldInitializeSpecieWithName()
    {
        // Arrange
        var name = "Dog";

        // Act
        var specie = Specie.Create(name);

        // Assert
        Assert.NotNull(specie);
        Assert.Equal(name, specie.Name.Value);
    }

    /// <summary>
    /// Tests that creating a specie with invalid name throws exception.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenNameIsInvalid()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => Specie.Create(string.Empty));
        Assert.Contains("Ім'я не може бути порожнім", ex.Message);
    }

    /// <summary>
    /// Tests renaming a specie updates the Name property and raises event.
    /// </summary>
    [Fact]
    public void Rename_ShouldUpdateNameAndAddEvent()
    {
        // Arrange
        var specie = Specie.Create("Cat");

        // Act
        specie.Rename("Kitten");

        // Assert
        Assert.Equal("Kitten", specie.Name.Value);
        var renameEvent = Assert.Single(specie.DomainEvents.OfType<SpecieRenamedEvent>());
        Assert.Equal("Kitten", renameEvent.newName);
    }

    /// <summary>
    /// Tests adding a breed to a specie succeeds and raises event.
    /// </summary>
    [Fact]
    public void AddBreed_ShouldAddBreedAndRaiseEvent()
    {
        // Arrange
        var specie = Specie.Create("Dog");
        var breed = Breed.Create(specie.Id, "Bulldog", "Description");

        // Act
        specie.AddBreed(breed);

        // Assert breed collection
        Assert.Single(specie.Breeds);
        Assert.Equal(breed.Id, specie.Breeds.First().Id);

        // Assert that a BreedAddedEvent exists among domain events
        var breedAddedEvent = Assert.Single(specie.DomainEvents, e => e is BreedAddedEvent);
        Assert.IsType<BreedAddedEvent>(breedAddedEvent);
    }

    /// <summary>
    /// Tests that adding a null breed throws ArgumentNullException.
    /// </summary>
    [Fact]
    public void AddBreed_ShouldThrow_WhenBreedIsNull()
    {
        // Arrange
        var specie = Specie.Create("Dog");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => specie.AddBreed(null!));
    }

    /// <summary>
    /// Tests that adding a duplicate breed throws InvalidOperationException.
    /// </summary>
    [Fact]
    public void AddBreed_ShouldThrow_WhenBreedAlreadyExists()
    {
        // Arrange
        var specie = Specie.Create("Dog");
        var breed = Breed.Create(specie.Id, "Bulldog", "Description");
        specie.AddBreed(breed);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => specie.AddBreed(breed));
    }

    /// <summary>
    /// Tests removing a breed succeeds and raises event.
    /// </summary>
    [Fact]
    public void RemoveBreed_ShouldRemoveBreedAndRaiseEvent()
    {
        // Arrange
        var specie = Specie.Create("Dog");
        var breed = Breed.Create(specie.Id, "Bulldog", "Description");
        specie.AddBreed(breed);

        // Act
        var result = specie.RemoveBreed(breed.Id);

        // Assert
        Assert.True(result);
        Assert.Empty(specie.Breeds);
        var lastEvent = Assert.Single(specie.DomainEvents, e => e is BreedRemovedEvent);
        Assert.IsType<BreedRemovedEvent>(lastEvent);
    }

    /// <summary>
    /// Tests removing a breed that does not exist returns false.
    /// </summary>
    [Fact]
    public void RemoveBreed_ShouldReturnFalse_WhenBreedDoesNotExist()
    {
        // Arrange
        var specie = Specie.Create("Dog");
        var nonExistentBreedId = Guid.NewGuid();

        // Act
        var result = specie.RemoveBreed(nonExistentBreedId);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests SpecieByNameSpecification filters correctly.
    /// </summary>
    [Fact]
    public void SpecieByNameSpecification_ShouldFilterByName()
    {
        // Arrange
        var dog = Specie.Create("Dog");
        var cat = Specie.Create("Cat");
        var list = new List<Specie> { dog, cat };
        var spec = new SpecieByNameSpecification("Dog");

        // Act
        var result = list.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Dog", result.First().Name.Value);
    }

    /// <summary>
    /// Tests that SpecieByNameSpecification throws exception on empty name.
    /// </summary>
    [Fact]
    public void SpecieByNameSpecification_ShouldThrow_OnEmptyName()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new SpecieByNameSpecification(string.Empty));
        Assert.Contains("Ім'я не може бути нульовим або порожнім", ex.Message);
    }
}
