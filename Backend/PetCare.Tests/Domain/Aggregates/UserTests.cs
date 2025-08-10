namespace PetCare.Tests.Domain.Aggregates;
using Moq;
using PetCare.Domain.Abstractions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="User"/> aggregate.
/// </summary>
public class UserTests
{
    private readonly Mock<IFileStorageService> fileStorageMock;
    private readonly User user;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserTests"/> class.
    /// </summary>
    public UserTests()
    {
        this.fileStorageMock = new Mock<IFileStorageService>();
        this.user = User.Create(
            email: "test@example.com",
            passwordHash: "hashedPassword",
            firstName: "John",
            lastName: "Doe",
            phone: "+12345678901",
            role: UserRole.User,
            preferences: new Dictionary<string, string> { { "theme", "dark" } },
            points: 100,
            lastLogin: DateTime.UtcNow.AddDays(-1),
            profilePhoto: "http://example.com/photo.jpg",
            language: "en");
    }

    /// <summary>
    /// Tests the successful creation of a <see cref="User"/> instance.
    /// </summary>
    [Fact]
    public void Create_ValidParameters_ReturnsUser()
    {
        // Arrange
        string email = "user@example.com";
        string passwordHash = "secureHash";
        string firstName = "Jane";
        string lastName = "Smith";
        string phone = "+12345678901";
        UserRole role = UserRole.User;
        var preferences = new Dictionary<string, string> { { "theme", "light" } };
        int points = 50;
        DateTime? lastLogin = DateTime.UtcNow;
        string profilePhoto = "http://example.com/photo.jpg";
        string language = "uk";

        // Act
        var user = User.Create(email, passwordHash, firstName, lastName, phone, role, preferences, points, lastLogin, profilePhoto, language);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(email, user.Email.Value);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(phone, user.Phone.Value);
        Assert.Equal(role, user.Role);
        Assert.Equal(preferences, user.Preferences);
        Assert.Equal(points, user.Points);
        Assert.Equal(lastLogin, user.LastLogin);
        Assert.Equal(profilePhoto, user.ProfilePhoto);
        Assert.Equal(language, user.Language);
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
        Assert.True(user.UpdatedAt <= DateTime.UtcNow);
    }

    /// <summary>
    /// Tests that <see cref="User.Create"/> throws an exception for invalid email.
    /// </summary>
    [Fact]
    public void Create_InvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        string invalidEmail = "invalid-email";

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            User.Create(invalidEmail, "hashedPassword", "John", "Doe", "+12345678901", UserRole.User));
    }

    /// <summary>
    /// Tests that <see cref="User.Create"/> throws an exception for empty password hash.
    /// </summary>
    [Fact]
    public void Create_EmptyPasswordHash_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            User.Create("test@example.com", "", "John", "Doe", "+12345678901", UserRole.User));
    }

    /// <summary>
    /// Tests updating the user's profile with valid parameters.
    /// </summary>
    [Fact]
    public void UpdateProfile_ValidParameters_UpdatesFields()
    {
        // Arrange
        string newFirstName = "Jane";
        string newLastName = "Smith";
        string newPhone = "+98765432109";
        string newProfilePhoto = "http://example.com/newphoto.jpg";
        string newLanguage = "en";
        Guid requestingUserId = this.user.Id;

        // Act
        this.user.UpdateProfile(newFirstName, newLastName, newPhone, newProfilePhoto, newLanguage, requestingUserId);

        // Assert
        Assert.Equal(newFirstName, this.user.FirstName);
        Assert.Equal(newLastName, this.user.LastName);
        Assert.Equal(newPhone, this.user.Phone.Value);
        Assert.Equal(newProfilePhoto, this.user.ProfilePhoto);
        Assert.Equal(newLanguage, this.user.Language);
        Assert.True(this.user.UpdatedAt > this.user.CreatedAt);
    }

    /// <summary>
    /// Tests that <see cref="User.UpdateProfile"/> throws an exception for unauthorized user.
    /// </summary>
    [Fact]
    public void UpdateProfile_UnauthorizedUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        Guid differentUserId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() =>
            this.user.UpdateProfile("Jane", "Smith", "+98765432109", null, "en", differentUserId));
    }

    /// <summary>
    /// Тестує асинхронне оновлення фото профілю.
    /// </summary>
    /// <returns>Повертає задачу <see cref="Task"/>, що представляє асинхронне виконання тесту.</returns>
    [Fact]
    public async Task UpdateProfilePhotoAsync_ValidParameters_UpdatesPhoto()
    {
        // Arrange
        var fileStream = new MemoryStream();
        string fileName = "photo.jpg";
        var config = new ProfilePhotoConfig(5000000, new[] { ".jpg" }); // Використовує конструктор ProfilePhotoConfig(long, string[])
        Guid requestingUserId = this.user.Id;
        string newPhotoUrl = "http://example.com/newphoto.jpg";
        this.fileStorageMock.Setup(fs => fs.UploadAsync(It.IsAny<Stream>(), fileName, config.maxSizeBytes, config.allowedExtensions))
            .ReturnsAsync(newPhotoUrl);
        this.fileStorageMock.Setup(fs => fs.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        await this.user.UpdateProfilePhotoAsync(this.fileStorageMock.Object, fileStream, fileName, config, requestingUserId);

        // Assert
        Assert.Equal(newPhotoUrl, this.user.ProfilePhoto);
        Assert.True(this.user.UpdatedAt > this.user.CreatedAt);
        this.fileStorageMock.Verify(fs => fs.UploadAsync(fileStream, fileName, config.maxSizeBytes, config.allowedExtensions), Times.Once());
        this.fileStorageMock.Verify(fs => fs.DeleteAsync(It.IsAny<string>()), Times.Once());
    }

    /// <summary>
    /// Tests adding points to the user by an admin.
    /// </summary>
    [Fact]
    public void AddPoints_AdminRole_AddsPoints()
    {
        // Arrange
        var adminUser = User.Create(
            "admin@example.com",
            "hashedPassword",
            "Admin",
            "User",
            "+12345678901",
            UserRole.Admin);
        int initialPoints = adminUser.Points;
        int pointsToAdd = 50;
        Guid requestingUserId = adminUser.Id;

        // Act
        adminUser.AddPoints(pointsToAdd, requestingUserId);

        // Assert
        Assert.Equal(initialPoints + pointsToAdd, adminUser.Points);
        Assert.True(adminUser.UpdatedAt > adminUser.CreatedAt);
    }

    /// <summary>
    /// Tests that <see cref="User.AddPoints"/> throws an exception for non-admin users.
    /// </summary>
    [Fact]
    public void AddPoints_NonAdmin_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        int pointsToAdd = 50;
        Guid requestingUserId = this.user.Id;

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => this.user.AddPoints(pointsToAdd, requestingUserId));
    }

    /// <summary>
    /// Tests deducting points from the user by an admin.
    /// </summary>
    [Fact]
    public void DeductPoints_ValidAmount_DeductsPoints()
    {
        // Arrange
        var adminUser = User.Create(
            "admin@example.com",
            "hashedPassword",
            "Admin",
            "User",
            "+12345678901",
            UserRole.Admin,
            points: 100);
        int pointsToDeduct = 50;
        Guid requestingUserId = adminUser.Id;

        // Act
        adminUser.DeductPoints(pointsToDeduct, requestingUserId);

        // Assert
        Assert.Equal(50, adminUser.Points);
        Assert.True(adminUser.UpdatedAt > adminUser.CreatedAt);
    }

    /// <summary>
    /// Tests that <see cref="User.DeductPoints"/> throws an exception for negative amount.
    /// </summary>
    [Fact]
    public void DeductPoints_NegativeAmount_ThrowsArgumentException()
    {
        // Arrange
        var adminUser = User.Create(
            "admin@example.com",
            "hashedPassword",
            "Admin",
            "User",
            "+12345678901",
            UserRole.Admin);
        Guid requestingUserId = adminUser.Id;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => adminUser.DeductPoints(-10, requestingUserId));
    }

    /// <summary>
    /// Tests adding a shelter subscription.
    /// </summary>
    [Fact]
    public void AddShelterSubscription_ValidSubscription_AddsSubscription()
    {
        // Arrange
        var subscription = ShelterSubscription.Create(this.user.Id, Guid.NewGuid());
        Guid requestingUserId = this.user.Id;

        // Act
        this.user.AddShelterSubscription(subscription, requestingUserId);

        // Assert
        Assert.Contains(subscription, this.user.ShelterSubscriptions);
        Assert.True(this.user.UpdatedAt > this.user.CreatedAt);
    }

    /// <summary>
    /// Tests that <see cref="User.AddShelterSubscription"/> throws an exception for duplicate subscription.
    /// </summary>
    [Fact]
    public void AddShelterSubscription_DuplicateSubscription_ThrowsInvalidOperationException()
    {
        // Arrange
        var shelterId = Guid.NewGuid();
        var subscription = ShelterSubscription.Create(this.user.Id, shelterId);
        Guid requestingUserId = this.user.Id;
        this.user.AddShelterSubscription(subscription, requestingUserId);

        // Act & Assert
        var duplicateSubscription = ShelterSubscription.Create(this.user.Id, shelterId);
        Assert.Throws<InvalidOperationException>(() => this.user.AddShelterSubscription(duplicateSubscription, requestingUserId));
    }

    /// <summary>
    /// Tests adding a gamification reward by an admin.
    /// </summary>
    [Fact]
    public void AddGamificationReward_AdminRole_AddsRewardAndPoints()
    {
        // Arrange
        var adminUser = User.Create(
            "admin@example.com",
            "hashedPassword",
            "Admin",
            "User",
            "+12345678901",
            UserRole.Admin,
            points: 100);
        var reward = GamificationReward.Create(
            userId: adminUser.Id,
            taskId: null,
            points: 50,
            description: "Test Reward");
        Guid requestingUserId = adminUser.Id;

        // Act
        adminUser.AddGamificationReward(reward, requestingUserId);

        // Assert
        Assert.Contains(reward, adminUser.GamificationRewards);
        Assert.Equal(150, adminUser.Points);
        Assert.True(adminUser.UpdatedAt > adminUser.CreatedAt);
    }

    /// <summary>
    /// Tests that <see cref="User.AddGamificationReward"/> throws an exception for non-admin users.
    /// </summary>
    [Fact]
    public void AddGamificationReward_NonAdmin_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var reward = GamificationReward.Create(
            userId: this.user.Id,
            taskId: null,
            points: 50,
            description: "Test Reward");
        Guid requestingUserId = this.user.Id;

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => this.user.AddGamificationReward(reward, requestingUserId));
    }

    /// <summary>
    /// Tests role-based permissions for creating articles.
    /// </summary>
    [Fact]
    public void CanCreateArticle_AnyRole_ReturnsTrue()
    {
        // Act & Assert
        Assert.True(this.user.CanCreateArticle());
    }

    /// <summary>
    /// Tests role-based permissions for deleting articles.
    /// </summary>
    [Fact]
    public void CanDeleteArticle_AdminRole_ReturnsTrue()
    {
        // Arrange
        var adminUser = User.Create(
            "admin@example.com",
            "hashedPassword",
            "Admin",
            "User",
            "+12345678901",
            UserRole.Admin);

        // Act & Assert
        Assert.True(adminUser.CanDeleteArticle());
    }

    /// <summary>
    /// Tests role-based permissions for moderating comments.
    /// </summary>
    [Fact]
    public void CanModerateComments_ModeratorRole_ReturnsTrue()
    {
        // Arrange
        var moderatorUser = User.Create(
            "moderator@example.com",
            "hashedPassword",
            "Moderator",
            "User",
            "+12345678901",
            UserRole.Moderator);

        // Act & Assert
        Assert.True(moderatorUser.CanModerateComments());
    }

    /// <summary>
    /// Tests changing the password with valid parameters.
    /// </summary>
    [Fact]
    public void ChangePassword_ValidParameters_ChangesPassword()
    {
        // Arrange
        string newPasswordHash = "newHashedPassword";
        Guid requestingUserId = this.user.Id;

        // Act
        this.user.ChangePassword(newPasswordHash, requestingUserId);

        // Assert
        Assert.Equal(newPasswordHash, this.user.PasswordHash);
        Assert.True(this.user.UpdatedAt > this.user.CreatedAt);
    }

    /// <summary>
    /// Tests that <see cref="User.ChangePassword"/> throws an exception for empty password hash.
    /// </summary>
    [Fact]
    public void ChangePassword_EmptyPasswordHash_ThrowsArgumentException()
    {
        // Arrange
        Guid requestingUserId = this.user.Id;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => this.user.ChangePassword("", requestingUserId));
    }
}

