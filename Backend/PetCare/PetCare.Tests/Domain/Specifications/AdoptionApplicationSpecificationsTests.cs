namespace PetCare.Tests.Domain.Specifications;

using System;
using System.Collections.Generic;
using System.Linq;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.Specifications.AdoptionApplication;

/// <summary>
/// Unit tests for adoption application specifications.
/// </summary>
public class AdoptionApplicationSpecificationsTests
{
    /// <summary>
    /// Tests that <see cref="AdoptionApplicationsByAnimalSpecification"/> returns applications matching the given animal ID.
    /// </summary>
    [Fact]
    public void AdoptionApplicationsByAnimalSpecification_ShouldMatchCorrectAnimalId()
    {
        // Arrange
        var animalId = Guid.NewGuid();
        var spec = new AdoptionApplicationsByAnimalSpecification(animalId);

        var apps = new List<AdoptionApplication>
        {
            AdoptionApplication.Create(Guid.NewGuid(), animalId, "Comment 1"),
            AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Comment 2"),
        };

        // Act
        var result = apps.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(animalId, result[0].AnimalId);
    }

    /// <summary>
    /// Tests that <see cref="AdoptionApplicationsByAnimalSpecification"/> throws exception when initialized with empty Guid.
    /// </summary>
    [Fact]
    public void AdoptionApplicationsByAnimalSpecification_ShouldThrow_WhenAnimalIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new AdoptionApplicationsByAnimalSpecification(Guid.Empty));
        Assert.Contains("Ідентифікатор тварини не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Tests that <see cref="AdoptionApplicationsByUserSpecification"/> returns applications matching the given user ID.
    /// </summary>
    [Fact]
    public void AdoptionApplicationsByUserSpecification_ShouldMatchCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var spec = new AdoptionApplicationsByUserSpecification(userId);

        var apps = new List<AdoptionApplication>
        {
            AdoptionApplication.Create(userId, Guid.NewGuid(), "Comment 1"),
            AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Comment 2"),
        };

        // Act
        var result = apps.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(userId, result[0].UserId);
    }

    /// <summary>
    /// Tests that <see cref="AdoptionApplicationsByUserSpecification"/> throws exception when initialized with empty Guid.
    /// </summary>
    [Fact]
    public void AdoptionApplicationsByUserSpecification_ShouldThrow_WhenUserIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new AdoptionApplicationsByUserSpecification(Guid.Empty));
        Assert.Contains("Ідентифікатор користувача не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Tests that <see cref="PendingAdoptionApplicationsSpecification"/> returns only applications with Pending status.
    /// </summary>
    [Fact]
    public void PendingAdoptionApplicationsSpecification_ShouldMatchOnlyPendingStatus()
    {
        // Arrange
        var spec = new PendingAdoptionApplicationsSpecification();

        var apps = new List<AdoptionApplication>
        {
            AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Comment 1"),
            AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Comment 2"),
            AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Comment 3"),
        };

        // Set different statuses manually for testing
        apps[1].Approve(Guid.NewGuid());
        apps[2].Reject("Some reason");

        // Act
        var result = apps.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.All(result, a => Assert.Equal(AdoptionStatus.Pending, a.Status));
    }
}