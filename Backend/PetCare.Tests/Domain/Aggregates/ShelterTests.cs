// <copyright file="ShelterTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;

using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Shelter"/> aggregate.
/// </summary>
public sealed class ShelterTests
{
    /// <summary>
    /// Verifies that a shelter is initialized correctly with valid data.
    /// </summary>
    [Fact]
    public void Create_WithValidData_ShouldInitializeProperly()
    {
        var shelter = CreateTestShelter();

        Assert.Equal("HAPPY PAWS", shelter.Name.Value.ToUpperInvariant());
        Assert.Equal(2, shelter.Photos.Count);
        Assert.Single(shelter.SocialMedia);
        Assert.Equal(3, shelter.CurrentOccupancy);
        Assert.Equal(10, shelter.Capacity);
    }

    /// <summary>
    /// Verifies that null collections in Create result in empty collections.
    /// </summary>
    [Fact]
    public void Create_WithNullCollections_ShouldUseEmpty()
    {
        var shelter = Shelter.Create(
            slug: "slug",
            name: "Name",
            address: "Address",
            coordinates: Coordinates.From(10, 10),
            contactPhone: "+380501111111",
            contactEmail: "test@example.com",
            description: null,
            capacity: 5,
            currentOccupancy: 2,
            photos: null,
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: Guid.NewGuid());

        Assert.Empty(shelter.Photos);
        Assert.Empty(shelter.SocialMedia);
    }

    /// <summary>
    /// Verifies that only provided properties are updated in shelter.
    /// </summary>
    [Fact]
    public void Update_WithNewNameAndAddress_ShouldUpdateOnlyThose()
    {
        var shelter = CreateTestShelter();
        var oldPhone = shelter.ContactPhone;
        var newName = "New Name";
        var newAddress = "456 Updated St";

        shelter.Update(name: newName, address: newAddress);

        Assert.Equal(newName, shelter.Name.Value);
        Assert.Equal(newAddress, shelter.Address.Value);
        Assert.Equal(oldPhone, shelter.ContactPhone);
    }

    /// <summary>
    /// Verifies that new photos and social media collections replace existing ones.
    /// </summary>
    [Fact]
    public void Update_WithNewPhotosAndSocialMedia_ShouldReplaceCollections()
    {
        var shelter = CreateTestShelter();
        var newPhotos = new List<string> { "new1.jpg", "new2.jpg" };
        var newSocial = new Dictionary<string, string> { { "Instagram", "insta.com/happypaws" } };

        shelter.Update(photos: newPhotos, socialMedia: newSocial);

        Assert.Equal(2, shelter.Photos.Count);
        Assert.Single(shelter.SocialMedia);
        Assert.Contains("new1.jpg", shelter.Photos);
        Assert.Contains("Instagram", shelter.SocialMedia.Keys);
    }

    /// <summary>
    /// Verifies that adding an animal increases occupancy and registers the ID.
    /// </summary>
    [Fact]
    public void AddAnimal_WithFreeCapacity_ShouldSucceed()
    {
        var shelter = CreateTestShelter();
        var newAnimalId = Guid.NewGuid();

        shelter.AddAnimal(newAnimalId);

        Assert.Contains(newAnimalId, shelter.AnimalIds);
        Assert.Equal(4, shelter.CurrentOccupancy);
    }

    /// <summary>
    /// Verifies that adding an animal when shelter is full throws an exception.
    /// </summary>
    [Fact]
    public void AddAnimal_WhenFull_ShouldThrow()
    {
        var shelter = Shelter.Create(
            slug: "slug",
            name: "Name",
            address: "Address",
            coordinates: Coordinates.From(10, 10),
            contactPhone: "+380501111111",
            contactEmail: "test@example.com",
            description: null,
            capacity: 1,
            currentOccupancy: 1,
            photos: null,
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: Guid.NewGuid());

        var animalId = Guid.NewGuid();

        var ex = Assert.Throws<InvalidOperationException>(() => shelter.AddAnimal(animalId));
        Assert.Contains("Притулок заповнений", ex.Message);
    }

    /// <summary>
    /// Verifies that adding the same animal twice throws an exception.
    /// </summary>
    [Fact]
    public void AddAnimal_AlreadyExists_ShouldThrow()
    {
        var shelter = CreateTestShelter();
        var existingId = Guid.NewGuid();
        shelter.AddAnimal(existingId);

        var ex = Assert.Throws<InvalidOperationException>(() => shelter.AddAnimal(existingId));
        Assert.Contains("Ця тварина вже перебуває у притулку", ex.Message);
    }

    /// <summary>
    /// Verifies that removing an existing animal decreases occupancy.
    /// </summary>
    [Fact]
    public void RemoveAnimal_ExistingAnimal_ShouldSucceed()
    {
        var shelter = CreateTestShelter();
        var animalId = Guid.NewGuid();
        shelter.AddAnimal(animalId);
        var before = shelter.CurrentOccupancy;

        shelter.RemoveAnimal(animalId);

        Assert.DoesNotContain(animalId, shelter.AnimalIds);
        Assert.Equal(before - 1, shelter.CurrentOccupancy);
    }

    /// <summary>
    /// Verifies that removing a non-existent animal throws an exception.
    /// </summary>
    [Fact]
    public void RemoveAnimal_NotFound_ShouldThrow()
    {
        var shelter = CreateTestShelter();
        var missingId = Guid.NewGuid();

        var ex = Assert.Throws<InvalidOperationException>(() => shelter.RemoveAnimal(missingId));
        Assert.Contains("Тварину не знайдено", ex.Message);
    }

    /// <summary>
    /// Verifies that HasFreeCapacity returns true when there is space.
    /// </summary>
    [Fact]
    public void HasFreeCapacity_WhenSpaceAvailable_ShouldReturnTrue()
    {
        var shelter = CreateTestShelter();

        Assert.True(shelter.HasFreeCapacity());
    }

    /// <summary>
    /// Verifies that HasFreeCapacity returns false when shelter is full.
    /// </summary>
    [Fact]
    public void HasFreeCapacity_WhenFull_ShouldReturnFalse()
    {
        var shelter = Shelter.Create(
            slug: "slug",
            name: "Full Shelter",
            address: "Full Street",
            coordinates: Coordinates.From(1, 1),
            contactPhone: "+380501111111",
            contactEmail: "full@example.com",
            description: null,
            capacity: 2,
            currentOccupancy: 2,
            photos: null,
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: Guid.NewGuid());

        Assert.False(shelter.HasFreeCapacity());
    }

    /// <summary>
    /// Creates a test shelter with predefined data.
    /// </summary>
    /// <returns>A new <see cref="Shelter"/> instance.</returns>
    private static Shelter CreateTestShelter()
    {
        return Shelter.Create(
            slug: "test-shelter",
            name: "Happy Paws",
            address: "123 Main St",
            coordinates: Coordinates.From(50.4501, 30.5234),
            contactPhone: "+380501234567",
            contactEmail: "info@happypaws.org",
            description: "A friendly shelter",
            capacity: 10,
            currentOccupancy: 3,
            photos: new List<string> { "photo1.jpg", "photo2.jpg" },
            virtualTourUrl: "https://tour.example.com",
            workingHours: "9:00 - 18:00",
            socialMedia: new Dictionary<string, string> { { "Facebook", "fb.com/happypaws" } },
            managerId: Guid.NewGuid());
    }
}
