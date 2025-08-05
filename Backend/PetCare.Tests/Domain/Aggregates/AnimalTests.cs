// <copyright file="AnimalTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="Animal"/> aggregate.
/// </summary>
public sealed class AnimalTests
{
    /// <summary>
    /// Verifies that creating an animal with valid data sets properties correctly.
    /// </summary>
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Act
        var animal = CreateTestAnimal();

        // Assert
        Assert.Equal("Барсик", animal.Name.Value);
        Assert.Equal(AnimalStatus.Available, animal.Status);
        Assert.Equal("1234567890", animal.MicrochipId?.Value);
        Assert.Contains("фото1.jpg", animal.Photos);
        Assert.Contains("відео1.mp4", animal.Videos);
        Assert.True(animal.IsSterilized);
        Assert.True(animal.HaveDocuments);
        Assert.True(animal.CanBeAdopted);
        Assert.True(animal.HasMicrochip);
    }

    /// <summary>
    /// Verifies that creating an animal with empty user ID throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithEmptyUserId_ShouldThrow()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Animal.Create("slug", Guid.Empty, "Ім'я", Guid.NewGuid(), null, AnimalGender.Male, null, null, null, null, Guid.NewGuid(), AnimalStatus.Available, null, null, 0, null, null, null, false, false));

        Assert.Contains("Ідентифікатор користувача не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Verifies that creating an animal with empty breed ID throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithEmptyBreedId_ShouldThrow()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Animal.Create("slug", Guid.NewGuid(), "Ім'я", Guid.Empty, null, AnimalGender.Male, null, null, null, null, Guid.NewGuid(), AnimalStatus.Available, null, null, 0, null, null, null, false, false));

        Assert.Contains("Ідентифікатор породи не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Verifies that creating an animal with empty shelter ID throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithEmptyShelterId_ShouldThrow()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Animal.Create("slug", Guid.NewGuid(), "Ім'я", Guid.NewGuid(), null, AnimalGender.Male, null, null, null, null, Guid.Empty, AnimalStatus.Available, null, null, 0, null, null, null, false, false));

        Assert.Contains("Ідентифікатор притулку не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Verifies that Update correctly changes selected fields.
    /// </summary>
    [Fact]
    public void Update_WithSelectedFields_ShouldUpdateAnimal()
    {
        var animal = CreateTestAnimal();

        animal.Update(name: "Рекс", weight: 18.2f, isSterilized: false);

        Assert.Equal("Рекс", animal.Name.Value);
        Assert.Equal(18.2f, animal.Weight);
        Assert.False(animal.IsSterilized);
    }

    /// <summary>
    /// Verifies that ChangeStatus updates the animal status.
    /// </summary>
    [Fact]
    public void ChangeStatus_ShouldUpdateStatus()
    {
        var animal = CreateTestAnimal();

        animal.ChangeStatus(AnimalStatus.Adopted);

        Assert.Equal(AnimalStatus.Adopted, animal.Status);
        Assert.False(animal.CanBeAdopted);
    }

    /// <summary>
    /// Verifies that AddPhoto adds a new photo.
    /// </summary>
    [Fact]
    public void AddPhoto_ShouldAddPhoto()
    {
        var animal = CreateTestAnimal();
        var url = "фото2.jpg";

        animal.AddPhoto(url);

        Assert.Contains(url, animal.Photos);
    }

    /// <summary>
    /// Verifies that RemovePhoto removes an existing photo.
    /// </summary>
    [Fact]
    public void RemovePhoto_ShouldRemovePhoto()
    {
        var animal = CreateTestAnimal();
        var existing = "фото1.jpg";

        animal.RemovePhoto(existing);

        Assert.DoesNotContain(existing, animal.Photos);
    }

    /// <summary>
    /// Verifies that AddVideo adds a new video.
    /// </summary>
    [Fact]
    public void AddVideo_ShouldAddVideo()
    {
        var animal = CreateTestAnimal();
        var url = "відео2.mp4";

        animal.AddVideo(url);

        Assert.Contains(url, animal.Videos);
    }

    /// <summary>
    /// Verifies that RemoveVideo removes an existing video.
    /// </summary>
    [Fact]
    public void RemoveVideo_ShouldRemoveVideo()
    {
        var animal = CreateTestAnimal();
        var existing = "відео1.mp4";

        animal.RemoveVideo(existing);

        Assert.DoesNotContain(existing, animal.Videos);
    }

    /// <summary>
    /// Verifies that ValidateAdoptionRequirements throws if requirements are too short.
    /// </summary>
    [Fact]
    public void ValidateAdoptionRequirements_WhenTooShort_ShouldThrow()
    {
        var animal = CreateTestAnimal();
        animal.Update(adoptionRequirements: "Замало");

        var ex = Assert.Throws<InvalidOperationException>(() => animal.ValidateAdoptionRequirements());
        Assert.Equal("Вимоги до адопції тварини не заповнені або занадто короткі.", ex.Message);
    }

    /// <summary>
    /// Verifies that ValidateAdoptionRequirements does not throw for valid input.
    /// </summary>
    [Fact]
    public void ValidateAdoptionRequirements_WhenValid_ShouldSucceed()
    {
        var animal = CreateTestAnimal();
        animal.Update(adoptionRequirements: "Просторий будинок, обгороджений двір, турботлива сімʼя");

        animal.ValidateAdoptionRequirements();
    }

    private static Animal CreateTestAnimal() =>
       Animal.Create(
           slug: "dobryj-pes",
           userId: Guid.NewGuid(),
           name: "Барсик",
           breedId: Guid.NewGuid(),
           birthday: Birthday.Create(new DateOnly(2020, 5, 1)),
           gender: AnimalGender.Male,
           description: "Дуже дружній і спокійний пес",
           healthStatus: "Здоровий",
           photos: new List<string> { "фото1.jpg" },
           videos: new List<string> { "відео1.mp4" },
           shelterId: Guid.NewGuid(),
           status: AnimalStatus.Available,
           adoptionRequirements: "Любляча родина з садом",
           microchipId: "1234567890",
           idNumber: 42,
           weight: 15.5f,
           height: 60.2f,
           color: "Коричневий",
           isSterilized: true,
           haveDocuments: true);
}
