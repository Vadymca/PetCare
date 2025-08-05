// <copyright file="AdoptionApplicationTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="AdoptionApplication"/> aggregate.
/// </summary>
public sealed class AdoptionApplicationTests
{
    /// <summary>
    /// Verifies that creating an application with valid data sets all fields correctly.
    /// </summary>
    [Fact]
    public void Create_WithValidParameters_ShouldCreatePendingApplication()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var animalId = Guid.NewGuid();
        var comment = "I love dogs";

        // Act
        var application = AdoptionApplication.Create(userId, animalId, comment);

        // Assert
        Assert.Equal(userId, application.UserId);
        Assert.Equal(animalId, application.AnimalId);
        Assert.Equal(comment, application.Comment);
        Assert.Equal(AdoptionStatus.Pending, application.Status);
        Assert.True(application.IsPending);
        Assert.NotEqual(default, application.ApplicationDate);
        Assert.NotEqual(default, application.CreatedAt);
        Assert.NotEqual(default, application.UpdatedAt);
    }

    /// <summary>
    /// Verifies that creating an application with an empty user ID throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithEmptyUserId_ShouldThrowArgumentException()
    {
        // Arrange
        var userId = Guid.Empty;
        var animalId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            AdoptionApplication.Create(userId, animalId, null));
        Assert.Equal("Ідентифікатор користувача не може бути порожнім. (Parameter 'userId')", ex.Message);
    }

    /// <summary>
    /// Verifies that creating an application with an empty animal ID throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithEmptyAnimalId_ShouldThrowArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var animalId = Guid.Empty;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            AdoptionApplication.Create(userId, animalId, null));
        Assert.Equal("Ідентифікатор тварини не може бути порожнім. (Parameter 'animalId')", ex.Message);
    }

    /// <summary>
    /// Verifies that a pending application can be approved correctly.
    /// </summary>
    [Fact]
    public void Approve_ShouldUpdateStatusAndApprover()
    {
        // Arrange
        var application = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        var adminId = Guid.NewGuid();

        // Act
        application.Approve(adminId);

        // Assert
        Assert.Equal(AdoptionStatus.Approved, application.Status);
        Assert.Equal(adminId, application.ApprovedBy);
        Assert.True(application.IsApproved);
        Assert.False(application.IsPending);
        Assert.False(application.IsRejected);
    }

    /// <summary>
    /// Verifies that approving a non-pending application throws an exception.
    /// </summary>
    [Fact]
    public void Approve_WhenNotPending_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var application = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        application.Reject("Reason");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            application.Approve(Guid.NewGuid()));
        Assert.Equal("Затверджуються лише ті заявки, які знаходяться на розгляді.", ex.Message);
    }

    /// <summary>
    /// Verifies that a pending application can be rejected correctly.
    /// </summary>
    [Fact]
    public void Reject_ShouldUpdateStatusAndReason()
    {
        // Arrange
        var application = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        var reason = "Відсутність належних умов для проживання";

        // Act
        application.Reject(reason);

        // Assert
        Assert.Equal(AdoptionStatus.Rejected, application.Status);
        Assert.Equal(reason, application.RejectionReason);
        Assert.True(application.IsRejected);
        Assert.False(application.IsApproved);
        Assert.False(application.IsPending);
    }

    /// <summary>
    /// Verifies that rejecting a non-pending application throws an exception.
    /// </summary>
    [Fact]
    public void Reject_WhenNotPending_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var application = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        application.Approve(Guid.NewGuid());

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            application.Reject("Reason"));
        Assert.Equal("Відхилити можна лише ті заявки, що перебувають на розгляді.", ex.Message);
    }

    /// <summary>
    /// Verifies that admin notes can be added and updated correctly.
    /// </summary>
    [Fact]
    public void AddAdminNotes_WithValidText_ShouldUpdateNotes()
    {
        // Arrange
        var application = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        var notes = "Перевірено адміністратором притулку";

        // Act
        application.AddAdminNotes(notes);

        // Assert
        Assert.Equal(notes, application.AdminNotes);
    }

    /// <summary>
    /// Verifies that adding empty admin notes throws an exception.
    /// </summary>
    [Fact]
    public void AddAdminNotes_WithEmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        var application = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            application.AddAdminNotes(" "));
        Assert.Equal("Адміністративні нотатки не можуть бути порожніми. (Parameter 'notes')", ex.Message);
    }
}
