namespace PetCare.Tests.Domain.Aggregates;

using FluentAssertions;
using Moq;
using PetCare.Domain.Abstractions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// Unit tests for <see cref="Shelter"/> aggregate.
/// </summary>
public class ShelterTests
{
    private readonly Guid validManagerId = Guid.NewGuid();
    private readonly Coordinates validCoordinates = Coordinates.From(50.0, 30.0);

    /// <summary>
    /// Ensures that a shelter is correctly created with all valid parameters.
    /// </summary>
    [Fact]
    public void Create_ShouldReturnShelter_WhenParametersAreValid()
    {
        var shelter = this.CreateDefaultShelter(
            slug: "valid-slug",
            name: "Shelter Name",
            address: "Address 123",
            contactPhone: "+380501234567",
            contactEmail: "test@shelter.com",
            description: "A nice shelter",
            capacity: 10,
            currentOccupancy: 5,
            photos: new List<string> { "photo1.jpg", "photo2.jpg" },
            virtualTourUrl: "https://tour.example.com",
            workingHours: "9:00-18:00",
            socialMedia: new Dictionary<string, string> { { "Facebook", "fb.com/shelter" } });
        shelter.Should().NotBeNull();
        shelter.Slug.Value.Should().StartWith("valid-slug");
        shelter.Name.Value.Should().Be("Shelter Name");
        shelter.Address.Value.Should().Be("Address 123");
        shelter.Coordinates.Should().Be(this.validCoordinates);
        shelter.ContactPhone.Value.Should().Be("+380501234567");
        shelter.ContactEmail.Value.Should().Be("test@shelter.com");
        shelter.Description.Should().Be("A nice shelter");
        shelter.Capacity.Should().Be(10);
        shelter.CurrentOccupancy.Should().Be(5);
        shelter.Photos.Should().BeEquivalentTo(new List<string> { "photo1.jpg", "photo2.jpg" });
        shelter.VirtualTourUrl.Should().Be("https://tour.example.com");
        shelter.WorkingHours.Should().Be("9:00-18:00");
        shelter.SocialMedia.Should().ContainKey("Facebook");
        shelter.ManagerId.Should().Be(this.validManagerId);
        shelter.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        shelter.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Ensures that updating a shelter changes its properties when valid data is provided.
    /// </summary>
    [Fact]
    public void Update_ShouldChangeProperties_WhenValidDataProvided()
    {
        var shelter = this.CreateDefaultShelter();

        shelter.Update(
            name: "New Shelter Name",
            address: "New Address 456",
            coordinates: Coordinates.From(51, 31),
            contactPhone: "+380509876543",
            contactEmail: "new@shelter.com",
            description: "Updated description",
            capacity: 20,
            currentOccupancy: 10,
            photos: new List<string> { "new1.jpg", "new2.jpg" },
            virtualTourUrl: "https://newtour.example.com",
            workingHours: "10:00-19:00",
            socialMedia: new Dictionary<string, string> { { "Instagram", "instagram.com/new" } });

        shelter.Name.Value.Should().Be("New Shelter Name");
        shelter.Address.Value.Should().Be("New Address 456");
        shelter.Coordinates.Should().Be(Coordinates.From(51, 31));
        shelter.ContactPhone.Value.Should().Be("+380509876543");
        shelter.ContactEmail.Value.Should().Be("new@shelter.com");
        shelter.Description.Should().Be("Updated description");
        shelter.Capacity.Should().Be(20);
        shelter.CurrentOccupancy.Should().Be(10);
        shelter.Photos.Should().BeEquivalentTo(new List<string> { "new1.jpg", "new2.jpg" });
        shelter.VirtualTourUrl.Should().Be("https://newtour.example.com");
        shelter.WorkingHours.Should().Be("10:00-19:00");
        shelter.SocialMedia.Should().ContainKey("Instagram");
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Ensures that updating a shelter with null or empty parameters does not change existing properties.
    /// </summary>
    [Fact]
    public void Update_ShouldNotChangeProperties_WhenParametersAreNullOrEmpty()
    {
        var shelter = this.CreateDefaultShelter();

        var originalName = shelter.Name;
        var originalAddress = shelter.Address;
        var originalCoordinates = shelter.Coordinates;
        var originalPhone = shelter.ContactPhone;
        var originalEmail = shelter.ContactEmail;
        var originalDescription = shelter.Description;
        var originalCapacity = shelter.Capacity;
        var originalOccupancy = shelter.CurrentOccupancy;
        var originalPhotos = shelter.Photos;
        var originalVirtualTourUrl = shelter.VirtualTourUrl;
        var originalWorkingHours = shelter.WorkingHours;
        var originalSocialMedia = shelter.SocialMedia;

        shelter.Update(
            name: string.Empty,
            address: " ",
            coordinates: null,
            contactPhone: null,
            contactEmail: string.Empty,
            description: null,
            capacity: null,
            currentOccupancy: null,
            photos: null,
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null);

        shelter.Name.Should().Be(originalName);
        shelter.Address.Should().Be(originalAddress);
        shelter.Coordinates.Should().Be(originalCoordinates);
        shelter.ContactPhone.Should().Be(originalPhone);
        shelter.ContactEmail.Should().Be(originalEmail);
        shelter.Description.Should().Be(originalDescription);
        shelter.Capacity.Should().Be(originalCapacity);
        shelter.CurrentOccupancy.Should().Be(originalOccupancy);
        shelter.Photos.Should().BeEquivalentTo(originalPhotos);
        shelter.VirtualTourUrl.Should().Be(originalVirtualTourUrl);
        shelter.WorkingHours.Should().Be(originalWorkingHours);
        shelter.SocialMedia.Should().BeEquivalentTo(originalSocialMedia);
    }

    /// <summary>
    /// Verifies that adding an animal increases occupancy when capacity is not exceeded.
    /// </summary>
    [Fact]
    public void AddAnimal_ShouldAddAnimal_WhenCapacityNotExceeded()
    {
        var shelter = this.CreateDefaultShelter(capacity: 2, currentOccupancy: 1);
        var animal = this.CreateTestAnimal(shelter.Id);

        shelter.AddAnimal(animal, this.validManagerId);

        shelter.Animals.Should().ContainSingle(a => a.Id == animal.Id);
        shelter.CurrentOccupancy.Should().Be(2);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Verifies that adding an animal throws an exception when shelter capacity is exceeded.
    /// </summary>
    [Fact]
    public void AddAnimal_ShouldThrow_WhenCapacityExceeded()
    {
        // Створюємо притулок з місткістю 1 і без тварин
        var shelter = this.CreateDefaultShelter(capacity: 1, currentOccupancy: 0);

        // Додаємо першу тварину — Shelter стає повним
        var firstAnimal = this.CreateTestAnimal(shelter.Id);
        shelter.AddAnimal(firstAnimal, this.validManagerId);

        // Додаємо другу тварину — має кинути виняток
        var secondAnimal = this.CreateTestAnimal(shelter.Id);
        Action act = () => shelter.AddAnimal(secondAnimal, this.validManagerId);

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("Притулок заповнений. Неможливо додати нову тварину.");
    }

    /// <summary>
    /// Verifies that adding the same animal twice throws an exception.
    /// </summary>
    [Fact]
    public void AddAnimal_ShouldThrow_WhenAnimalAlreadyAdded()
    {
        var shelter = this.CreateDefaultShelter(capacity: 2, currentOccupancy: 0);
        var animal = this.CreateTestAnimal(shelter.Id);
        shelter.AddAnimal(animal, this.validManagerId);

        Action act = () => shelter.AddAnimal(animal, this.validManagerId);

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("Ця тварина вже є у притулку.");
    }

    /// <summary>
    /// Verifies that removing an existing animal decreases occupancy.
    /// </summary>
    [Fact]
    public void RemoveAnimal_ShouldRemoveAnimal_WhenExists()
    {
        var shelter = this.CreateDefaultShelter(capacity: 5, currentOccupancy: 0);
        var animal = this.CreateTestAnimal(shelter.Id);
        shelter.AddAnimal(animal, this.validManagerId);

        shelter.RemoveAnimal(animal.Id, this.validManagerId);

        shelter.Animals.Should().NotContain(a => a.Id == animal.Id);
        shelter.CurrentOccupancy.Should().Be(0);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Verifies that removing a non-existing animal throws an exception.
    /// </summary>
    [Fact]
    public void RemoveAnimal_ShouldThrow_WhenAnimalNotFound()
    {
        var shelter = this.CreateDefaultShelter();

        Action act = () => shelter.RemoveAnimal(Guid.NewGuid(), this.validManagerId);

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("Тварину не знайдено у притулку.");
    }

    /// <summary>
    /// Verifies that <see cref="Shelter.HasFreeCapacity"/> returns true when there is free space.
    /// </summary>
    [Fact]
    public void HasFreeCapacity_ShouldReturnTrue_WhenCapacityAvailable()
    {
        var shelter = this.CreateDefaultShelter(capacity: 5, currentOccupancy: 3);

        shelter.HasFreeCapacity().Should().BeTrue();
    }

    /// <summary>
    /// Verifies that <see cref="Shelter.HasFreeCapacity"/> returns false when shelter is full.
    /// </summary>
    [Fact]
    public void HasFreeCapacity_ShouldReturnFalse_WhenCapacityFull()
    {
        var shelter = this.CreateDefaultShelter(capacity: 3, currentOccupancy: 3);

        shelter.HasFreeCapacity().Should().BeFalse();
    }

    /// <summary>
    /// Verifies that a new social media platform is added correctly.
    /// </summary>
    [Fact]
    public void AddOrUpdateSocialMedia_ShouldAddPlatform_WhenValidData()
    {
        var shelter = this.CreateDefaultShelter();
        shelter.AddOrUpdateSocialMedia("Twitter", "https://twitter.com/shelter");

        shelter.SocialMedia.Should().ContainKey("Twitter");
        shelter.SocialMedia["Twitter"].Should().Be("https://twitter.com/shelter");
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Verifies that an existing social media platform is updated correctly.
    /// </summary>
    [Fact]
    public void AddOrUpdateSocialMedia_ShouldUpdatePlatform_WhenAlreadyExists()
    {
        var shelter = this.CreateDefaultShelter();
        shelter.AddOrUpdateSocialMedia("Facebook", "https://facebook.com/old");
        shelter.AddOrUpdateSocialMedia("Facebook", "https://facebook.com/new");

        shelter.SocialMedia["Facebook"].Should().Be("https://facebook.com/new");
    }

    /// <summary>
    /// Verifies that adding a social media platform with invalid name throws an exception.
    /// </summary>
    /// <param name="invalidPlatform">The invalid social media platform name (null, empty, or whitespace).</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AddOrUpdateSocialMedia_ShouldThrow_WhenPlatformInvalid(string? invalidPlatform)
    {
        var shelter = this.CreateDefaultShelter();

        Action act = () => shelter.AddOrUpdateSocialMedia(invalidPlatform!, "https://url.com");

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Назва платформи не може бути порожньою.*");
    }

    /// <summary>
    /// Verifies that adding a social media platform with invalid URL throws an exception.
    /// </summary>
    /// <param name="invalidUrl">The invalid URL to test (null, empty, or whitespace).</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AddOrUpdateSocialMedia_ShouldThrow_WhenUrlInvalid(string? invalidUrl)
    {
        var shelter = this.CreateDefaultShelter();

        Action act = () => shelter.AddOrUpdateSocialMedia("Facebook", invalidUrl!);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("URL не може бути порожнім.*");
    }

    /// <summary>
    /// Verifies that removing an existing social media platform succeeds.
    /// </summary>
    [Fact]
    public void RemoveSocialMedia_ShouldRemovePlatform_WhenExists()
    {
        var shelter = this.CreateDefaultShelter();
        shelter.AddOrUpdateSocialMedia("Instagram", "https://instagram.com/shelter");

        var removed = shelter.RemoveSocialMedia("Instagram");

        removed.Should().BeTrue();
        shelter.SocialMedia.Should().NotContainKey("Instagram");
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Verifies that removing a non-existing social media platform returns false.
    /// </summary>
    [Fact]
    public void RemoveSocialMedia_ShouldReturnFalse_WhenPlatformNotExists()
    {
        var shelter = this.CreateDefaultShelter();

        shelter.RemoveSocialMedia("NonExisting").Should().BeFalse();
    }

    /// <summary>
    /// Verifies that adding a photo uploads it successfully and updates the shelter's photo list.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task AddPhotoAsync_ShouldAddPhotoUrl_WhenUploadSucceeds()
    {
        var fileStorageMock = new Mock<IFileStorageService>();
        fileStorageMock
            .Setup(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string[]>()))
            .ReturnsAsync("uploaded-photo-url");

        var shelter = this.CreateDefaultShelter();

        var mediaConfig = new MediaConfig(1024 * 1024, new[] { ".jpg" });
        using var stream = new MemoryStream(new byte[10]);

        await shelter.AddPhotoAsync(fileStorageMock.Object, stream, "photo.jpg", 10, mediaConfig);

        shelter.Photos.Should().Contain("uploaded-photo-url");
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
        fileStorageMock.Verify(x => x.UploadAsync(It.IsAny<Stream>(), "photo.jpg", mediaConfig.maxSizeBytes, mediaConfig.allowedExtensions), Times.Once);
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentNullException"/> is thrown when the file stream is null.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task AddPhotoAsync_ShouldThrow_WhenFileStreamIsNull()
    {
        var shelter = this.CreateDefaultShelter();
        var mediaConfig = new MediaConfig(1024 * 1024, new[] { ".jpg" });

        Func<Task> act = () => shelter.AddPhotoAsync(Mock.Of<IFileStorageService>(), null!, "file.jpg", 10, mediaConfig);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that adding a photo with an invalid file name throws an <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="invalidFileName">The invalid file name to test (null, empty, or whitespace).</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task AddPhotoAsync_ShouldThrow_WhenFileNameInvalid(string? invalidFileName)
    {
        var shelter = this.CreateDefaultShelter();
        var mediaConfig = new MediaConfig(1024 * 1024, new[] { ".jpg" });
        using var stream = new MemoryStream(new byte[10]);

        Func<Task> act = () => shelter.AddPhotoAsync(Mock.Of<IFileStorageService>(), stream, invalidFileName!, 10, mediaConfig);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Ім'я файлу не може бути порожнім.*");
    }

    /// <summary>
    /// Verifies that removing an existing photo deletes it from the shelter and calls the file storage service.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task RemovePhotoAsync_ShouldRemovePhoto_WhenPhotoExists()
    {
        var fileStorageMock = new Mock<IFileStorageService>();
        fileStorageMock.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        var shelter = this.CreateDefaultShelter();
        shelter.Update(photos: new List<string> { "photo-to-remove.jpg" });

        var result = await shelter.RemovePhotoAsync(fileStorageMock.Object, "photo-to-remove.jpg");

        result.Should().BeTrue();
        shelter.Photos.Should().NotContain("photo-to-remove.jpg");
        fileStorageMock.Verify(x => x.DeleteAsync("photo-to-remove.jpg"), Times.Once);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Verifies that removing a photo with an invalid URL returns false.
    /// </summary>
    /// <param name="invalidPhotoUrl">The invalid photo URL to test (null, empty, or whitespace).</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task RemovePhotoAsync_ShouldReturnFalse_WhenPhotoUrlInvalid(string? invalidPhotoUrl)
    {
        var shelter = this.CreateDefaultShelter();

        var result = await shelter.RemovePhotoAsync(Mock.Of<IFileStorageService>(), invalidPhotoUrl!);

        result.Should().BeFalse();
    }

    private Animal CreateTestAnimal(Guid shelterId)
    {
        return Animal.Create(
            slug: "test-animal-slug",
            userId: Guid.NewGuid(),
            name: "Test Animal",
            breedId: Guid.NewGuid(),
            birthday: null,
            gender: AnimalGender.Unknown,
            description: null,
            healthStatus: null,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: shelterId,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 1,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);
    }

    private Shelter CreateDefaultShelter(
     string slug = "default-slug",
     string name = "Default Name",
     string address = "Default Address",
     string contactPhone = "+380501112233",
     string contactEmail = "default@shelter.com",
     string description = "Default description",
     int capacity = 10,
     int currentOccupancy = 0,
     List<string>? photos = null,
     string virtualTourUrl = "https://defaulttour.com",
     string workingHours = "9-17",
     Dictionary<string, string>? socialMedia = null)
    {
        var shelter = Shelter.Create(
            slug,
            name,
            address,
            this.validCoordinates,
            contactPhone,
            contactEmail,
            description,
            capacity,
            0,
            photos ?? new List<string> { "photo1.jpg" },
            virtualTourUrl,
            workingHours,
            socialMedia ?? new Dictionary<string, string> { { "Facebook", "fb.com/default" } },
            this.validManagerId
        );

        for (int i = 0; i < currentOccupancy; i++)
        {
            var animal = this.CreateTestAnimal(shelter.Id);
            shelter.AddAnimal(animal, this.validManagerId);
        }

        return shelter;
    }
}
