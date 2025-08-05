// <copyright file="LostPetTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;

using NetTopologySuite.Geometries;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="LostPet"/> aggregate.
/// </summary>
public sealed class LostPetTests
{
    /// <summary>
    /// Verifies that creating a lost pet with valid data sets properties correctly.
    /// </summary>
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        var slug = "lost-pet-slug";
        var userId = Guid.NewGuid();
        var breedId = Guid.NewGuid();
        var name = "Вусик";
        var description = "Чорний кіт з білою плямою";
        var lastSeenLocation = new Point(30, 50) { SRID = 4326 };
        var lastSeenDate = DateTime.UtcNow.AddDays(-5);
        var photos = new List<string> { "photo1.jpg", "photo2.jpg" };
        var status = LostPetStatus.Lost;
        var adminNotes = "Перевірено адміністратором";
        decimal? reward = 100m;
        var contactAlternative = "0971234567";
        var microchipId = "123456789";
        var lostPet = LostPet.Create(
            slug,
            userId,
            breedId,
            name,
            description,
            lastSeenLocation,
            lastSeenDate,
            photos,
            status,
            adminNotes,
            reward,
            contactAlternative,
            microchipId);

        Assert.Equal(slug, lostPet.Slug.Value);
        Assert.Equal(userId, lostPet.UserId);
        Assert.Equal(breedId, lostPet.BreedId);
        Assert.Equal(name, lostPet.Name?.Value);
        Assert.Equal(description, lostPet.Description);
        Assert.Equal(lastSeenLocation, lostPet.LastSeenLocation);
        Assert.Equal(lastSeenDate, lostPet.LastSeenDate);
        Assert.Equal(photos.Count, lostPet.Photos.Count);
        foreach (var photo in photos)
        {
            Assert.Contains(photo, lostPet.Photos);
        }

        Assert.Equal(status, lostPet.Status);
        Assert.Equal(adminNotes, lostPet.AdminNotes);
        Assert.Equal(reward, lostPet.Reward);
        Assert.Equal(contactAlternative, lostPet.ContactAlternative);
        Assert.Equal(microchipId, lostPet.MicrochipId?.Value);
        Assert.InRange(lostPet.CreatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that creating with empty user ID throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithEmptyUserId_ShouldThrow()
    {
        var ex = Assert.Throws<ArgumentException>(() => LostPet.Create(
            "slug",
            Guid.Empty,
            null,
            null,
            null,
            null,
            null,
            null,
            LostPetStatus.Lost,
            null,
            null,
            null,
            null));

        Assert.Contains("Ідентифікатор користувача не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Verifies that creating with negative reward throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithNegativeReward_ShouldThrow()
    {
        var ex = Assert.Throws<ArgumentException>(() => LostPet.Create(
            "slug",
            Guid.NewGuid(),
            null,
            null,
            null,
            null,
            null,
            null,
            LostPetStatus.Lost,
            null,
            -10,
            null,
            null));

        Assert.Contains("Розмір винагороди не може бути від’ємним.", ex.Message);
    }

    /// <summary>
    /// Verifies that adding a valid photo updates the collection and UpdatedAt.
    /// </summary>
    [Fact]
    public void AddPhoto_WithValidUrl_ShouldAddPhoto()
    {
        var lostPet = CreateTestLostPet();
        var photoUrl = "newphoto.jpg";

        lostPet.AddPhoto(photoUrl);

        Assert.Contains(photoUrl, lostPet.Photos);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that adding a null or whitespace photo URL throws an exception.
    /// </summary>
    /// <param name="invalidUrl">The invalid photo URL input, which can be null, empty, or whitespace.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AddPhoto_WithInvalidUrl_ShouldThrow(string? invalidUrl)
    {
        var lostPet = CreateTestLostPet();

        var ex = Assert.Throws<ArgumentException>(() => lostPet.AddPhoto(invalidUrl!));
        Assert.Contains("URL фотографії не може бути пустим.", ex.Message);
    }

    /// <summary>
    /// Verifies that removing a photo updates the collection and UpdatedAt.
    /// </summary>
    [Fact]
    public void RemovePhoto_WithExistingPhoto_ShouldRemovePhoto()
    {
        var lostPet = CreateTestLostPet();
        var photoToRemove = lostPet.Photos[0];

        lostPet.RemovePhoto(photoToRemove);

        Assert.DoesNotContain(photoToRemove, lostPet.Photos);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that removing a non-existing photo does not update UpdatedAt.
    /// </summary>
    [Fact]
    public void RemovePhoto_WithNonExistingPhoto_ShouldNotUpdate()
    {
        var lostPet = CreateTestLostPet();
        var oldUpdatedAt = lostPet.UpdatedAt;

        lostPet.RemovePhoto("nonexistent.jpg");

        Assert.Equal(oldUpdatedAt, lostPet.UpdatedAt);
    }

    /// <summary>
    /// Verifies updating the name changes the property and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateName_ShouldChangeName()
    {
        var lostPet = CreateTestLostPet();
        var newName = "Мурчик";

        lostPet.UpdateName(newName);

        Assert.Equal(newName, lostPet.Name?.Value);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that updating the name with null or whitespace clears the name.
    /// </summary>
    /// <param name="input">The input string to update the name with. Can be null, empty, or whitespace.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateName_WithNullOrWhitespace_ShouldClearName(string? input)
    {
        var lostPet = CreateTestLostPet();

        lostPet.UpdateName(input);

        Assert.Null(lostPet.Name);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies updating the description changes the property and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateDescription_ShouldChangeDescription()
    {
        var lostPet = CreateTestLostPet();
        var newDescription = "Новий опис";

        lostPet.UpdateDescription(newDescription);

        Assert.Equal(newDescription, lostPet.Description);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies updating the last seen location and date changes properties and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateLastSeen_ShouldChangeLocationAndDate()
    {
        var lostPet = CreateTestLostPet();
        var newLocation = new Point(31, 51) { SRID = 4326 };
        var newDate = DateTime.UtcNow;

        lostPet.UpdateLastSeen(newLocation, newDate);

        Assert.Equal(newLocation, lostPet.LastSeenLocation);
        Assert.Equal(newDate, lostPet.LastSeenDate);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies updating the alternative contact changes the property and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateContactAlternative_ShouldChangeContact()
    {
        var lostPet = CreateTestLostPet();
        var newContact = "0987654321";

        lostPet.UpdateContactAlternative(newContact);

        Assert.Equal(newContact, lostPet.ContactAlternative);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that updating reward with zero value updates the property and UpdatedAt timestamp.
    /// </summary>
    [Fact]
    public void UpdateReward_WithZero_ShouldUpdate()
    {
        // Arrange: Create a test LostPet instance and set reward to zero.
        var lostPet = CreateTestLostPet();
        decimal? reward = 0m;

        // Act: Update the reward.
        lostPet.UpdateReward(reward);

        // Assert: The reward property is updated correctly.
        Assert.Equal(reward, lostPet.Reward);

        // Assert: UpdatedAt is set to a recent timestamp (within last 1 minute).
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that updating reward with a positive decimal value updates the property and UpdatedAt timestamp.
    /// </summary>
    [Fact]
    public void UpdateReward_WithPositiveDecimal_ShouldUpdate()
    {
        // Arrange: Create a test LostPet instance and set reward to a positive decimal value.
        var lostPet = CreateTestLostPet();
        decimal? reward = 50.5m;

        // Act: Update the reward.
        lostPet.UpdateReward(reward);

        // Assert: The reward property is updated correctly.
        Assert.Equal(reward, lostPet.Reward);

        // Assert: UpdatedAt is set to a recent timestamp (within last 1 minute).
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that updating reward with null clears the reward property and updates UpdatedAt timestamp.
    /// </summary>
    [Fact]
    public void UpdateReward_WithNull_ShouldUpdate()
    {
        // Arrange: Create a test LostPet instance and set reward to null.
        var lostPet = CreateTestLostPet();
        decimal? reward = null;

        // Act: Update the reward.
        lostPet.UpdateReward(reward);

        // Assert: The reward property is cleared (null).
        Assert.Null(lostPet.Reward);

        // Assert: UpdatedAt is set to a recent timestamp (within last 1 minute).
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies updating reward with negative value throws exception.
    /// </summary>
    [Fact]
    public void UpdateReward_WithNegativeValue_ShouldThrow()
    {
        var lostPet = CreateTestLostPet();

        var ex = Assert.Throws<ArgumentException>(() => lostPet.UpdateReward(-1));
        Assert.Contains("Розмір винагороди не може бути від’ємним.", ex.Message);
    }

    /// <summary>
    /// Verifies updating microchip ID updates property and UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateMicrochipId_ShouldChangeMicrochip()
    {
        var lostPet = CreateTestLostPet();
        var newMicrochipId = "NEWCHIP12345";

        lostPet.UpdateMicrochipId(newMicrochipId);

        Assert.Equal(newMicrochipId, lostPet.MicrochipId?.Value);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies updating microchip ID with null clears the microchip.
    /// </summary>
    [Fact]
    public void UpdateMicrochipId_WithNull_ShouldClearMicrochip()
    {
        var lostPet = CreateTestLostPet();

        lostPet.UpdateMicrochipId(null);

        Assert.Null(lostPet.MicrochipId);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that UpdateStatus changes status and optionally admin notes, and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateStatus_ShouldChangeStatusAndAdminNotes()
    {
        var lostPet = CreateTestLostPet();
        var newStatus = LostPetStatus.Found;
        var newAdminNotes = "Notes updated";

        lostPet.UpdateStatus(newStatus, newAdminNotes);

        Assert.Equal(newStatus, lostPet.Status);
        Assert.Equal(newAdminNotes, lostPet.AdminNotes);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that UpdateStatus changes status but keeps admin notes unchanged if null passed.
    /// </summary>
    [Fact]
    public void UpdateStatus_ShouldChangeStatusOnlyIfAdminNotesNull()
    {
        var lostPet = CreateTestLostPet();
        var newStatus = LostPetStatus.Found;
        var originalNotes = lostPet.AdminNotes;

        lostPet.UpdateStatus(newStatus, null);

        Assert.Equal(newStatus, lostPet.Status);
        Assert.Equal(originalNotes, lostPet.AdminNotes);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that setting a new microchip updates the microchip and UpdatedAt.
    /// </summary>
    [Fact]
    public void SetMicrochip_WithNewValue_ShouldSetMicrochip()
    {
        var lostPet = CreateTestLostPet();
        string validMicrochip = "ABC123"; // Валідний ID мікрочіпа за правилом

        lostPet.SetMicrochip(validMicrochip);

        Assert.Equal(validMicrochip.ToUpperInvariant(), lostPet.MicrochipId?.Value);
    }

    /// <summary>
    /// Verifies that setting the same microchip twice throws an exception.
    /// </summary>
    [Fact]
    public void SetMicrochip_WithSameValue_ShouldThrow()
    {
        var lostPet = CreateTestLostPet();
        var microchipValue = lostPet.MicrochipId!.Value;

        var ex = Assert.Throws<InvalidOperationException>(() => lostPet.SetMicrochip(microchipValue));
        Assert.Contains("Цей мікрочіп вже призначено цьому запису.", ex.Message);
    }

    /// <summary>
    /// Verifies that ClearMicrochip clears the microchip and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void ClearMicrochip_ShouldClearMicrochip()
    {
        var lostPet = CreateTestLostPet();

        lostPet.ClearMicrochip();

        Assert.Null(lostPet.MicrochipId);
        Assert.InRange(lostPet.UpdatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    private static LostPet CreateTestLostPet()
    {
        return LostPet.Create(
            slug: "test-slug",
            userId: Guid.NewGuid(),
            breedId: Guid.NewGuid(),
            name: "TestName",
            description: "Test description",
            lastSeenLocation: new Point(10, 10) { SRID = 4326 },
            lastSeenDate: DateTime.UtcNow.AddDays(-1),
            photos: new List<string> { "photo1.jpg", "photo2.jpg" },
            status: LostPetStatus.Lost,
            adminNotes: "Initial notes",
            reward: 50m,
            contactAlternative: "1234567890",
            microchipId: "microchip1");
    }
}