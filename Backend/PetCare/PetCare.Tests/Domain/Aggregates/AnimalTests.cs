namespace PetCare.Tests.Domain.Aggregates;

using System;
using System.Collections.Generic;
using FluentAssertions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using Xunit;

/// <summary>
/// Unit tests for <see cref="Animal"/> aggregate.
/// </summary>
public class AnimalTests
{
    private readonly Guid validUserId = Guid.NewGuid();
    private readonly Guid validBreedId = Guid.NewGuid();
    private readonly Guid validShelterId = Guid.NewGuid();

    /// <summary>
    /// Tests that <see cref="Animal.Create"/> creates an instance correctly with valid parameters.
    /// </summary>
    [Fact]
    public void Create_ShouldReturnValidAnimal_WhenParametersAreValid()
    {
        // Arrange
        var validUserId = Guid.NewGuid();
        var validBreedId = Guid.NewGuid();
        var validShelterId = Guid.NewGuid();
        string expectedSlugStart = "unique-slug";

        // Act
        var animal = Animal.Create(
            userId: validUserId,
            name: "TestName",
            breedId: validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthConditions: new List<string>(),
            specialNeeds: new List<string>(),
            temperaments: new List<AnimalTemperament>(),
            size: AnimalSize.Medium,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: validShelterId,
            status: AnimalStatus.Available,
            careCost: AnimalCareCost.SixHundred,
            adoptionRequirements: null,
            microchipId: null,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            isUnderCare: false,
            haveDocuments: false);

        // Assert
        animal.Slug.Value.Should().StartWith(expectedSlugStart);
        animal.Slug.Value.Should().MatchRegex(@"^unique-slug-[a-z0-9]{6}$");

        animal.UserId.Should().Be(validUserId);
        animal.BreedId.Should().Be(validBreedId);
        animal.ShelterId.Should().Be(validShelterId);
        animal.Name.Value.Should().Be("TestName");
    }

    /// <summary>
    /// Tests that <see cref="Animal.Create"/> throws <see cref="ArgumentException"/> when UserId is empty.
    /// </summary>
    [Fact]
    public void Create_ShouldThrowArgumentException_WhenUserIdIsEmpty()
    {
        Action act = () => Animal.Create(
            userId: Guid.Empty,
            name: "Name",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthConditions: new List<string>(),
            specialNeeds: new List<string>(),
            temperaments: new List<AnimalTemperament>(),
            size: AnimalSize.Medium,
            photos: null,
            videos: null,
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            careCost: AnimalCareCost.SixHundred,
            adoptionRequirements: null,
            microchipId: null,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            isUnderCare: false,
            haveDocuments: false);

        act.Should().Throw<ArgumentException>().WithMessage("*користувача не може бути порожнім*");
    }

    /// <summary>
    /// Tests that calling <see cref="Animal.Update"/> updates mutable properties correctly.
    /// </summary>
    [Fact]
    public void Update_ShouldModifyProperties_WhenValidValuesProvided()
    {
        var animal = Animal.Create(
            userId: this.validUserId,
            name: "OldName",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthConditions: new List<string>(),
            specialNeeds: new List<string>(),
            temperaments: new List<AnimalTemperament>(),
            size: AnimalSize.Medium,
            photos: null,
            videos: null,
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            careCost: AnimalCareCost.SixHundred,
            adoptionRequirements: null,
            microchipId: null,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            isUnderCare: false,
            haveDocuments: false);

        animal.Update(
            name: "NewName",
            description: "New description",
            weight: 10.0f,
            isSterilized: true);

        animal.Name.Value.Should().Be("NewName");
        animal.Description.Should().Be("New description");
        animal.Weight.Should().Be(10.0f);
        animal.IsSterilized.Should().BeTrue();
    }

    /// <summary>
    /// Tests that <see cref="Animal.ChangeStatus"/> updates the status and UpdatedAt.
    /// </summary>
    [Fact]
    public void ChangeStatus_ShouldUpdateStatusAndUpdatedAt()
    {
        var animal = Animal.Create(
            userId: this.validUserId,
            name: "Name",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthConditions: new List<string>(),
            specialNeeds: new List<string>(),
            temperaments: new List<AnimalTemperament>(),
            size: AnimalSize.Medium,
            photos: null,
            videos: null,
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            careCost: AnimalCareCost.SixHundred,
            adoptionRequirements: null,
            microchipId: null,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            isUnderCare: false,
            haveDocuments: false);

        var oldUpdatedAt = animal.UpdatedAt;
        animal.ChangeStatus(AnimalStatus.Adopted);

        animal.Status.Should().Be(AnimalStatus.Adopted);
        animal.UpdatedAt.Should().BeAfter(oldUpdatedAt);
    }

    /// <summary>
    /// Tests that <see cref="Animal.ValidateAdoptionRequirements"/> throws if requirements are null or too short.
    /// </summary>
    [Fact]
    public void ValidateAdoptionRequirements_ShouldThrow_WhenRequirementsAreInvalid()
    {
        var animal = Animal.Create(
            userId: this.validUserId,
            name: "Name",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthConditions: new List<string>(),
            specialNeeds: new List<string>(),
            temperaments: new List<AnimalTemperament>(),
            size: AnimalSize.Medium,
            photos: null,
            videos: null,
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            careCost: AnimalCareCost.SixHundred,
            adoptionRequirements: "short",
            microchipId: null,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            isUnderCare: false,
            haveDocuments: false);

        Action act = () => animal.ValidateAdoptionRequirements();

        act.Should().Throw<InvalidOperationException>().WithMessage("*Вимоги до адопції тварини*");
    }
}
