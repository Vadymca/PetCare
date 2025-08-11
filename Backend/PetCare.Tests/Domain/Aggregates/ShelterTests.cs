namespace PetCare.Tests.Domain.Aggregates;
using FluentAssertions;
using Moq;
using PetCare.Domain.Abstractions;
using PetCare.Domain.Aggregates;
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
    /// Tests creation of <see cref="Shelter"/> with valid parameters.
    /// </summary>
    [Fact]
    public void Create_ShouldReturnShelter_WhenParametersAreValid()
    {
        // Arrange
        var slug = "valid-slug";
        var name = "Shelter Name";
        var address = "Address 123";
        var contactPhone = "+380501234567";
        var contactEmail = "test@shelter.com";
        var photos = new List<string> { "photo1.jpg", "photo2.jpg" };
        var socialMedia = new Dictionary<string, string> { { "Facebook", "fb.com/shelter" } };
        var capacity = 10;
        var currentOccupancy = 5;
        var virtualTourUrl = "https://tour.example.com";
        var workingHours = "9:00-18:00";

        // Act
        var shelter = Shelter.Create(
            slug,
            name,
            address,
            this.validCoordinates,
            contactPhone,
            contactEmail,
            "A nice shelter",
            capacity,
            currentOccupancy,
            photos,
            virtualTourUrl,
            workingHours,
            socialMedia,
            this.validManagerId);

        // Assert
        shelter.Should().NotBeNull();
        shelter.Slug.Value.Should().StartWith("valid-slug");
        shelter.Name.Value.Should().Be(name);
        shelter.Address.Value.Should().Be(address);
        shelter.Coordinates.Should().Be(this.validCoordinates);
        shelter.ContactPhone.Value.Should().Be(contactPhone);
        shelter.ContactEmail.Value.Should().Be(contactEmail);
        shelter.Description.Should().Be("A nice shelter");
        shelter.Capacity.Should().Be(capacity);
        shelter.CurrentOccupancy.Should().Be(currentOccupancy);
        shelter.Photos.Should().BeEquivalentTo(photos);
        shelter.VirtualTourUrl.Should().Be(virtualTourUrl);
        shelter.WorkingHours.Should().Be(workingHours);
        shelter.SocialMedia.Should().BeEquivalentTo(socialMedia);
        shelter.ManagerId.Should().Be(this.validManagerId);
        shelter.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        shelter.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests updating shelter with some parameters.
    /// </summary>
    [Fact]
    public void Update_ShouldChangeProperties_WhenValidDataProvided()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();

        var newName = "New Shelter Name";
        var newAddress = "New Address 456";
        var newCoordinates = Coordinates.From(51.0, 31.0);
        var newPhone = "+380509876543";
        var newEmail = "new@shelter.com";
        var newDescription = "Updated description";
        var newCapacity = 20;
        var newOccupancy = 10;
        var newPhotos = new List<string> { "new1.jpg", "new2.jpg" };
        var newVirtualTourUrl = "https://newtour.example.com";
        var newWorkingHours = "10:00-19:00";
        var newSocialMedia = new Dictionary<string, string> { { "Instagram", "instagram.com/new" } };

        // Act
        shelter.Update(
            name: newName,
            address: newAddress,
            coordinates: newCoordinates,
            contactPhone: newPhone,
            contactEmail: newEmail,
            description: newDescription,
            capacity: newCapacity,
            currentOccupancy: newOccupancy,
            photos: newPhotos,
            virtualTourUrl: newVirtualTourUrl,
            workingHours: newWorkingHours,
            socialMedia: newSocialMedia);

        // Assert
        shelter.Name.Value.Should().Be(newName);
        shelter.Address.Value.Should().Be(newAddress);
        shelter.Coordinates.Should().Be(newCoordinates);
        shelter.ContactPhone.Value.Should().Be(newPhone);
        shelter.ContactEmail.Value.Should().Be(newEmail);
        shelter.Description.Should().Be(newDescription);
        shelter.Capacity.Should().Be(newCapacity);
        shelter.CurrentOccupancy.Should().Be(newOccupancy);
        shelter.Photos.Should().BeEquivalentTo(newPhotos);
        shelter.VirtualTourUrl.Should().Be(newVirtualTourUrl);
        shelter.WorkingHours.Should().Be(newWorkingHours);
        shelter.SocialMedia.Should().BeEquivalentTo(newSocialMedia);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Tests updating shelter with null or empty parameters does not change those fields.
    /// </summary>
    [Fact]
    public void Update_ShouldNotChangeProperties_WhenParametersAreNullOrEmpty()
    {
        // Arrange
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

        // Act
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

        // Assert
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
    /// Tests adding animal to shelter increases occupancy.
    /// </summary>
    [Fact]
    public void AddAnimal_ShouldAddAnimal_WhenCapacityNotExceeded()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter(capacity: 2, currentOccupancy: 1);
        var animalId = Guid.NewGuid();

        // Act
        shelter.AddAnimal(animalId);

        // Assert
        shelter.AnimalIds.Should().Contain(animalId);
        shelter.CurrentOccupancy.Should().Be(2);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Tests adding animal throws if capacity exceeded.
    /// </summary>
    [Fact]
    public void AddAnimal_ShouldThrow_WhenCapacityExceeded()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter(capacity: 1, currentOccupancy: 1);
        var animalId = Guid.NewGuid();

        // Act
        Action act = () => shelter.AddAnimal(animalId);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Притулок заповнений. Неможливо додати нову тварину.");
    }

    /// <summary>
    /// Tests adding the same animal twice throws.
    /// </summary>
    [Fact]
    public void AddAnimal_ShouldThrow_WhenAnimalAlreadyAdded()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter(capacity: 2, currentOccupancy: 0);
        var animalId = Guid.NewGuid();
        shelter.AddAnimal(animalId);

        // Act
        Action act = () => shelter.AddAnimal(animalId);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Ця тварина вже перебуває у притулку.");
    }

    /// <summary>
    /// Tests removing animal decreases occupancy.
    /// </summary>
    [Fact]
    public void RemoveAnimal_ShouldRemoveAnimal_WhenExists()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter(capacity: 5, currentOccupancy: 0);
        var animalId = Guid.NewGuid();
        shelter.AddAnimal(animalId);

        // Act
        shelter.RemoveAnimal(animalId);

        // Assert
        shelter.AnimalIds.Should().NotContain(animalId);
        shelter.CurrentOccupancy.Should().Be(0);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Tests removing animal throws if animal not found.
    /// </summary>
    [Fact]
    public void RemoveAnimal_ShouldThrow_WhenAnimalNotFound()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();

        // Act
        Action act = () => shelter.RemoveAnimal(Guid.NewGuid());

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Тварину не знайдено у притулку.");
    }

    /// <summary>
    /// Tests HasFreeCapacity returns true when occupancy less than capacity.
    /// </summary>
    [Fact]
    public void HasFreeCapacity_ShouldReturnTrue_WhenCapacityAvailable()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter(capacity: 5, currentOccupancy: 3);

        // Act
        var hasCapacity = shelter.HasFreeCapacity();

        // Assert
        hasCapacity.Should().BeTrue();
    }

    /// <summary>
    /// Tests HasFreeCapacity returns false when occupancy equals capacity.
    /// </summary>
    [Fact]
    public void HasFreeCapacity_ShouldReturnFalse_WhenCapacityFull()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter(capacity: 3, currentOccupancy: 3);

        // Act
        var hasCapacity = shelter.HasFreeCapacity();

        // Assert
        hasCapacity.Should().BeFalse();
    }

    /// <summary>
    /// Tests AddOrUpdateSocialMedia adds a new platform.
    /// </summary>
    [Fact]
    public void AddOrUpdateSocialMedia_ShouldAddPlatform_WhenValidData()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();
        var platform = "Twitter";
        var url = "https://twitter.com/shelter";

        // Act
        shelter.AddOrUpdateSocialMedia(platform, url);

        // Assert
        shelter.SocialMedia.Should().ContainKey(platform);
        shelter.SocialMedia[platform].Should().Be(url);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Tests AddOrUpdateSocialMedia updates existing platform url.
    /// </summary>
    [Fact]
    public void AddOrUpdateSocialMedia_ShouldUpdatePlatform_WhenAlreadyExists()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();
        var platform = "Facebook";
        var url1 = "https://facebook.com/old";
        var url2 = "https://facebook.com/new";

        shelter.AddOrUpdateSocialMedia(platform, url1);

        // Act
        shelter.AddOrUpdateSocialMedia(platform, url2);

        // Assert
        shelter.SocialMedia[platform].Should().Be(url2);
    }

    /// <summary>
    /// Tests AddOrUpdateSocialMedia throws when platform is null or whitespace.
    /// </summary>
    /// <param name="invalidPlatform">Invalid platform name (null, empty, or whitespace).</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AddOrUpdateSocialMedia_ShouldThrow_WhenPlatformInvalid(string? invalidPlatform)
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();

        // Act
        Action act = () => shelter.AddOrUpdateSocialMedia(invalidPlatform!, "https://url.com");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Назва платформи не може бути порожньою.*");
    }

    /// <summary>
    /// Tests AddOrUpdateSocialMedia throws when url is null or whitespace.
    /// </summary>
    /// <param name="invalidUrl">Invalid URL value (null, empty, or whitespace).</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AddOrUpdateSocialMedia_ShouldThrow_WhenUrlInvalid(string? invalidUrl)
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();

        // Act
        Action act = () => shelter.AddOrUpdateSocialMedia("Facebook", invalidUrl!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("URL не може бути порожнім.*");
    }

    /// <summary>
    /// Tests RemoveSocialMedia removes platform if exists.
    /// </summary>
    [Fact]
    public void RemoveSocialMedia_ShouldRemovePlatform_WhenExists()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();
        var platform = "Instagram";
        shelter.AddOrUpdateSocialMedia(platform, "https://instagram.com/shelter");

        // Act
        var removed = shelter.RemoveSocialMedia(platform);

        // Assert
        removed.Should().BeTrue();
        shelter.SocialMedia.Should().NotContainKey(platform);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Tests RemoveSocialMedia returns false when platform does not exist.
    /// </summary>
    [Fact]
    public void RemoveSocialMedia_ShouldReturnFalse_WhenPlatformNotExists()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();

        // Act
        var removed = shelter.RemoveSocialMedia("NonExisting");

        // Assert
        removed.Should().BeFalse();
    }

    /// <summary>
    /// Tests that <see cref="Shelter.AddPhotoAsync"/> adds the photo URL to the shelter's photos
    /// after a successful upload via the file storage service.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddPhotoAsync_ShouldAddPhotoUrl_WhenUploadSucceeds()
    {
        // Arrange
        var fileStorageMock = new Mock<IFileStorageService>();
        fileStorageMock
            .Setup(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string[]>()))
            .ReturnsAsync("uploaded-photo-url");

        var shelter = this.CreateDefaultShelter();

        var mediaConfig = new MediaConfig(1024 * 1024, new[] { ".jpg" });
        using var stream = new MemoryStream(new byte[10]);

        // Act
        await shelter.AddPhotoAsync(fileStorageMock.Object, stream, "photo.jpg", 10, mediaConfig);

        // Assert
        shelter.Photos.Should().Contain("uploaded-photo-url");
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
        fileStorageMock.Verify(x => x.UploadAsync(It.IsAny<Stream>(), "photo.jpg", mediaConfig.maxSizeBytes, mediaConfig.allowedExtensions), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="Shelter.AddPhotoAsync"/> throws an <see cref="ArgumentNullException"/>
    /// when the file stream parameter is null.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddPhotoAsync_ShouldThrow_WhenFileStreamIsNull()
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();
        var mediaConfig = new MediaConfig(1024 * 1024, new[] { ".jpg" });

        // Act
        Func<Task> act = () => shelter.AddPhotoAsync(Mock.Of<IFileStorageService>(), null!, "file.jpg", 10, mediaConfig);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Tests AddPhotoAsync throws when file name is null or whitespace.
    /// </summary>
    /// <param name="invalidFileName">Invalid file name value (null, empty, or whitespace).</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous test operation.</returns>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task AddPhotoAsync_ShouldThrow_WhenFileNameInvalid(string? invalidFileName)
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();
        var mediaConfig = new MediaConfig(1024 * 1024, new[] { ".jpg" });
        using var stream = new MemoryStream(new byte[10]);

        // Act
        Func<Task> act = () => shelter.AddPhotoAsync(Mock.Of<IFileStorageService>(), stream, invalidFileName!, 10, mediaConfig);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Ім'я файлу не може бути порожнім.*");
    }

    /// <summary>
    /// Tests RemovePhotoAsync removes photo and calls DeleteAsync.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test operation.</returns>
    [Fact]
    public async Task RemovePhotoAsync_ShouldRemovePhoto_WhenPhotoExists()
    {
        // Arrange
        var fileStorageMock = new Mock<IFileStorageService>();
        fileStorageMock.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        var shelter = this.CreateDefaultShelter();
        shelter.Update(photos: new List<string> { "photo-to-remove.jpg" });

        // Act
        var result = await shelter.RemovePhotoAsync(fileStorageMock.Object, "photo-to-remove.jpg");

        // Assert
        result.Should().BeTrue();
        shelter.Photos.Should().NotContain("photo-to-remove.jpg");
        fileStorageMock.Verify(x => x.DeleteAsync("photo-to-remove.jpg"), Times.Once);
        shelter.UpdatedAt.Should().BeAfter(shelter.CreatedAt);
    }

    /// <summary>
    /// Tests RemovePhotoAsync returns false if photoUrl is null or whitespace.
    /// </summary>
    /// <param name="invalidPhotoUrl">Invalid photo URL (null, empty, or whitespace).</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous test operation.</returns>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task RemovePhotoAsync_ShouldReturnFalse_WhenPhotoUrlInvalid(string? invalidPhotoUrl)
    {
        // Arrange
        var shelter = this.CreateDefaultShelter();

        // Act
        var result = await shelter.RemovePhotoAsync(Mock.Of<IFileStorageService>(), invalidPhotoUrl!);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Creates a default shelter instance for testing.
    /// </summary>
    /// <param name="capacity">Optional capacity override.</param>
    /// <param name="currentOccupancy">Optional current occupancy override.</param>
    /// <returns>New <see cref="Shelter"/> instance.</returns>
    private Shelter CreateDefaultShelter(int capacity = 10, int currentOccupancy = 0)
    {
        return Shelter.Create(
            "default-slug",
            "Default Name",
            "Default Address",
            this.validCoordinates,
            "+380501112233",
            "default@shelter.com",
            "Default description",
            capacity,
            currentOccupancy,
            new List<string> { "photo1.jpg" },
            "https://defaulttour.com",
            "9-17",
            new Dictionary<string, string> { { "Facebook", "fb.com/default" } },
            this.validManagerId);
    }
}
