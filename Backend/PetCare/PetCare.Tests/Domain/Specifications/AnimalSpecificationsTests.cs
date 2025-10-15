namespace PetCare.Tests.Domain.Specifications;

using System;
using System.Collections.Generic;
using System.Linq;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.Specifications.Animal;

/// <summary>
/// Unit tests for animal specifications.
/// </summary>
public class AnimalSpecificationsTests
{
    /// <summary>
    /// Tests that <see cref="AnimalsByBreedSpecification"/> returns only animals with the matching breed.
    /// </summary>
    [Fact]
    public void AnimalsByBreedSpecification_ShouldReturnOnlyMatchingBreed()
    {
        // Arrange
        var breedId = Guid.NewGuid();
        var spec = new AnimalsByBreedSpecification(breedId);

        var animals = new List<Animal>
        {
            this.CreateAnimal(breedId: breedId),
            this.CreateAnimal(breedId: Guid.NewGuid()),
        };

        // Act
        var result = animals.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(breedId, result[0].BreedId);
    }

    /// <summary>
    /// Tests that <see cref="AnimalsByShelterSpecification"/> returns only animals in the specified shelter.
    /// </summary>
    [Fact]
    public void AnimalsByShelterSpecification_ShouldReturnOnlyMatchingShelter()
    {
        // Arrange
        var shelterId = Guid.NewGuid();
        var spec = new AnimalsByShelterSpecification(shelterId);

        var animals = new List<Animal>
        {
            this.CreateAnimal(shelterId: shelterId),
            this.CreateAnimal(shelterId: Guid.NewGuid()),
        };

        // Act
        var result = animals.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(shelterId, result[0].ShelterId);
    }

    /// <summary>
    /// Tests that <see cref="AvailableAnimalsSpecification"/> returns only animals with status Available.
    /// </summary>
    [Fact]
    public void AvailableAnimalsSpecification_ShouldReturnOnlyAvailableAnimals()
    {
        // Arrange
        var spec = new AvailableAnimalsSpecification();

        var animals = new List<Animal>
        {
            this.CreateAnimal(status: AnimalStatus.Available),
            this.CreateAnimal(status: AnimalStatus.Adopted),
            this.CreateAnimal(status: AnimalStatus.Reserved),
        };

        // Act
        var result = animals.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.All(result, a => Assert.Equal(AnimalStatus.Available, a.Status));
    }

    /// <summary>
    /// Tests that <see cref="AnimalsByBreedSpecification"/> throws an exception when initialized with an empty GUID.
    /// </summary>
    [Fact]
    public void AnimalsByBreedSpecification_ShouldThrow_OnEmptyGuid()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new AnimalsByBreedSpecification(Guid.Empty));
        Assert.Contains("Ідентифікатор породи не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Tests that <see cref="AnimalsByShelterSpecification"/> throws an exception when initialized with an empty GUID.
    /// </summary>
    [Fact]
    public void AnimalsByShelterSpecification_ShouldThrow_OnEmptyGuid()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new AnimalsByShelterSpecification(Guid.Empty));
        Assert.Contains("Ідентифікатор притулку не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Creates a new test animal with optional breed, shelter, and status.
    /// </summary>
    /// <param name="breedId">Optional breed ID. If null, a new GUID will be generated.</param>
    /// <param name="shelterId">Optional shelter ID. If null, a new GUID will be generated.</param>
    /// <param name="status">The status of the animal. Defaults to <see cref="AnimalStatus.Available"/>.</param>
    /// <returns>A new <see cref="Animal"/> instance.</returns>
    private Animal CreateAnimal(
        Guid? breedId = null,
        Guid? shelterId = null,
        AnimalStatus status = AnimalStatus.Available)
    {
        return Animal.Create(
            userId: Guid.NewGuid(),
            name: "TestAnimal",
            breedId: breedId ?? Guid.NewGuid(),
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthConditions: new List<string>(),
            specialNeeds: new List<string>(),
            temperaments: new List<AnimalTemperament>(),
            size: AnimalSize.Medium,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: shelterId ?? Guid.NewGuid(),
            status: status,
            careCost: AnimalCareCost.SixHundred,
            adoptionRequirements: null,
            microchipId: null,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            isUnderCare: false,
            haveDocuments: false);
    }
}
