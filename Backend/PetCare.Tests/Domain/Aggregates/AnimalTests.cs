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
            slug: expectedSlugStart,
            userId: validUserId,
            name: "TestName",
            breedId: validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: validShelterId,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 123,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
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
            slug: "slug",
            userId: Guid.Empty,
            name: "Name",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: null,
            videos: null,
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 0,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
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
            slug: "slug",
            userId: this.validUserId,
            name: "OldName",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: null,
            videos: null,
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 0,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
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
            slug: "slug",
            userId: this.validUserId,
            name: "Name",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: null,
            videos: null,
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 0,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
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
            slug: "slug",
            userId: this.validUserId,
            name: "Name",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: null,
            videos: null,
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            adoptionRequirements: "short",
            microchipId: null,
            idNumber: 0,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);

        Action act = () => animal.ValidateAdoptionRequirements();

        act.Should().Throw<InvalidOperationException>().WithMessage("*Вимоги до адопції тварини*");
    }

    /// <summary>
    /// Tests adding a photo to the animal using a mock <see cref="IFileStorageService"/>.
    /// Verifies that after a successful upload, the photo URL is added to the animal's photos collection.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddPhotoAsync_ShouldAddPhotoUrl_WhenUploadSucceeds()
    {
        var fileStorageMock = new Mock<IFileStorageService>();
        fileStorageMock
            .Setup(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string[]>()))
            .ReturnsAsync("uploaded-photo-url");

        var animal = Animal.Create(
            slug: "slug",
            userId: this.validUserId,
            name: "Name",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 0,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);

        var mediaConfig = new MediaConfig(maxSizeBytes: 1024 * 1024, allowedExtensions: new[] { ".jpg" });

        await animal.AddPhotoAsync(fileStorageMock.Object, new MemoryStream(new byte[10]), "photo.jpg", 10, mediaConfig);

        animal.Photos.Should().Contain("uploaded-photo-url");
    }

    /// <summary>
    /// Tests removing a photo with mock <see cref="IFileStorageService"/>.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task RemovePhotoAsync_ShouldRemovePhotoUrl_WhenPhotoExists()
    {
        var fileStorageMock = new Mock<IFileStorageService>();
        fileStorageMock.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        var animal = Animal.Create(
            slug: "slug",
            userId: this.validUserId,
            name: "Name",
            breedId: this.validBreedId,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: new List<string> { "photo-to-remove" },
            videos: new List<string>(),
            shelterId: this.validShelterId,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 0,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);

        var removed = await animal.RemovePhotoAsync(fileStorageMock.Object, "photo-to-remove");

        removed.Should().BeTrue();
        animal.Photos.Should().NotContain("photo-to-remove");
        fileStorageMock.Verify(x => x.DeleteAsync("photo-to-remove"), Times.Once);
    }
}
