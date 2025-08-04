// <copyright file="AnimalAidRequestTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="AnimalAidRequest"/> aggregate.
/// </summary>
public sealed class AnimalAidRequestTests
{
    /// <summary>
    /// Tests creating a request with valid data initializes properties correctly.
    /// </summary>
    [Fact]
    public void Create_WithValidData_ShouldInitializeProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();
        var title = "Допомога тваринам";
        var description = "Потрібна термінова допомога";
        var category = AidCategory.Medical;
        var status = AidStatus.Open;  // змінив на Open
        decimal? estimatedCost = 1500m;
        var photos = new List<string> { "photo1.jpg" };

        // Act
        var request = AnimalAidRequest.Create(
            userId,
            shelterId,
            title,
            description,
            category,
            status,
            estimatedCost,
            photos);

        // Assert
        Assert.Equal(title, request.Title.Value);
        Assert.Equal(description, request.Description);
        Assert.Equal(category, request.Category);
        Assert.Equal(status, request.Status);
        Assert.Equal(estimatedCost, request.EstimatedCost);
        Assert.Equal(photos, request.Photos);
        Assert.Equal(userId, request.UserId);
        Assert.Equal(shelterId, request.ShelterId);
        Assert.True((DateTime.UtcNow - request.CreatedAt).TotalSeconds < 5);
        Assert.True((DateTime.UtcNow - request.UpdatedAt).TotalSeconds < 5);
    }

    /// <summary>
    /// Tests creating a request with negative estimated cost throws exception.
    /// </summary>
    [Fact]
    public void Create_WithNegativeEstimatedCost_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        decimal? negativeCost = -10m;

        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            AnimalAidRequest.Create(
                null,
                null,
                "Тест",
                null,
                AidCategory.Food,
                AidStatus.Open,
                negativeCost,
                null));

        Assert.Contains("Орієнтовна вартість має бути невід'ємною", ex.Message);
    }

    /// <summary>
    /// Tests UpdateStatus correctly changes status and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateStatus_ShouldChangeStatusAndUpdateTimestamp()
    {
        // Arrange
        var request = CreateTestRequest();
        var newStatus = AidStatus.Fulfilled;
        var oldUpdatedAt = request.UpdatedAt;

        // Act
        request.UpdateStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, request.Status);
        Assert.True(request.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests UpdateEstimatedCost with valid value updates property and UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateEstimatedCost_WithValidValue_ShouldUpdateProperty()
    {
        // Arrange
        var request = CreateTestRequest();
        decimal? newCost = 2000m;
        var oldUpdatedAt = request.UpdatedAt;

        // Act
        request.UpdateEstimatedCost(newCost);

        // Assert
        Assert.Equal(newCost, request.EstimatedCost);
        Assert.True(request.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests UpdateEstimatedCost with negative value throws exception.
    /// </summary>
    [Fact]
    public void UpdateEstimatedCost_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var request = CreateTestRequest();
        decimal? negativeCost = -5m;

        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => request.UpdateEstimatedCost(negativeCost));
        Assert.Contains("Вартість повинна бути невід'ємною.", ex.Message);
    }

    /// <summary>
    /// Tests AddPhoto adds photo URL and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void AddPhoto_WithValidUrl_ShouldAddPhoto()
    {
        // Arrange
        var request = CreateTestRequest();
        var photoUrl = "newphoto.jpg";
        var oldUpdatedAt = request.UpdatedAt;

        // Act
        request.AddPhoto(photoUrl);

        // Assert
        Assert.Contains(photoUrl, request.Photos);
        Assert.True(request.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests AddPhoto with null or empty URL throws exception.
    /// </summary>
    /// <param name="invalidUrl">The invalid URL value to test (null, empty, or whitespace).</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void AddPhoto_WithNullOrEmptyUrl_ShouldThrowArgumentException(string? invalidUrl)
    {
        // Arrange
        var request = CreateTestRequest();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => request.AddPhoto(invalidUrl!));
        Assert.Contains("URL не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Tests RemovePhoto removes existing photo and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void RemovePhoto_WithExistingPhoto_ShouldRemoveAndReturnTrue()
    {
        // Arrange
        var request = CreateTestRequest();
        var photoUrl = "photo1.jpg";

        // Act
        var result = request.RemovePhoto(photoUrl);

        // Assert
        Assert.True(result);
        Assert.DoesNotContain(photoUrl, request.Photos);
    }

    /// <summary>
    /// Tests RemovePhoto with non-existing photo returns false and does not update UpdatedAt.
    /// </summary>
    [Fact]
    public void RemovePhoto_WithNonExistingPhoto_ShouldReturnFalse()
    {
        // Arrange
        var request = CreateTestRequest();
        var nonExistingPhoto = "nonexistent.jpg";
        var oldUpdatedAt = request.UpdatedAt;

        // Act
        var result = request.RemovePhoto(nonExistingPhoto);

        // Assert
        Assert.False(result);
        Assert.Equal(oldUpdatedAt, request.UpdatedAt);
    }

    private static AnimalAidRequest CreateTestRequest()
    {
        return AnimalAidRequest.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Тестова допомога",
            "Опис тестової допомоги",
            AidCategory.Medical,
            AidStatus.Open,
            500m,
            new List<string> { "photo1.jpg" });
    }
}
