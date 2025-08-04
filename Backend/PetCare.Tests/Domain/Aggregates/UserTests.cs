// <copyright file="UserTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using System;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="User"/> aggregate.
/// </summary>
public sealed class UserTests
{
    /// <summary>
    /// Verifies that creating a user with valid data succeeds and populates all fields correctly.
    /// </summary>
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        var user = CreateTestUser();

        Assert.Equal("user@example.com", user.Email.Value);
        Assert.Equal("John", user.FirstName);
        Assert.Equal(UserRole.User, user.Role);
        Assert.Equal(0, user.Points);
        Assert.Equal("uk", user.Language);
    }

    /// <summary>
    /// Verifies that updating a user's profile correctly updates the fields provided.
    /// </summary>
    [Fact]
    public void UpdateProfile_ShouldUpdateFields()
    {
        var user = CreateTestUser();
        user.UpdateProfile("Alice", "Smith", "+380987654321", "photo.jpg", "en");

        Assert.Equal("Alice", user.FirstName);
        Assert.Equal("Smith", user.LastName);
        Assert.Equal("+380987654321", user.Phone.Value);
        Assert.Equal("photo.jpg", user.ProfilePhoto);
        Assert.Equal("en", user.Language);
    }

    /// <summary>
    /// Verifies that adding a positive amount of points increases the user's total points.
    /// </summary>
    [Fact]
    public void AddPoints_PositiveAmount_ShouldIncreasePoints()
    {
        var user = CreateTestUser();
        user.AddPoints(10);
        Assert.Equal(10, user.Points);
    }

    /// <summary>
    /// Verifies that adding a negative amount of points does not change the user's total points.
    /// </summary>
    [Fact]
    public void AddPoints_NegativeAmount_ShouldNotChangePoints()
    {
        var user = CreateTestUser();
        user.AddPoints(-5);
        Assert.Equal(0, user.Points);
    }

    /// <summary>
    /// Verifies that deducting a valid amount of points reduces the user's total points.
    /// </summary>
    [Fact]
    public void DeductPoints_ValidAmount_ShouldReducePoints()
    {
        var user = CreateTestUser();
        user.AddPoints(20);
        user.DeductPoints(5);
        Assert.Equal(15, user.Points);
    }

    /// <summary>
    /// Verifies that attempting to deduct more points than available throws an exception.
    /// </summary>
    [Fact]
    public void DeductPoints_TooMuch_ShouldThrow()
    {
        var user = CreateTestUser();
        user.AddPoints(3);
        Assert.Throws<ArgumentException>(() => user.DeductPoints(5));
    }

    /// <summary>
    /// Verifies that adding a new shelter subscription works correctly.
    /// </summary>
    [Fact]
    public void AddShelterSubscription_New_ShouldSucceed()
    {
        var user = CreateTestUser();
        var shelterId = Guid.NewGuid();
        var sub = ShelterSubscription.Create(user.Id, shelterId);

        user.AddShelterSubscription(sub);
        Assert.Contains(user.ShelterSubscriptions, s => s.ShelterId == shelterId);
    }

    /// <summary>
    /// Verifies that adding a duplicate shelter subscription throws an exception.
    /// </summary>
    [Fact]
    public void AddShelterSubscription_Existing_ShouldThrow()
    {
        var user = CreateTestUser();
        var shelterId = Guid.NewGuid();
        var sub = ShelterSubscription.Create(user.Id, shelterId);
        user.AddShelterSubscription(sub);

        Assert.Throws<InvalidOperationException>(() => user.AddShelterSubscription(sub));
    }

    /// <summary>
    /// Verifies that removing an existing shelter subscription works and removes it from the collection.
    /// </summary>
    [Fact]
    public void RemoveShelterSubscription_Existing_ShouldSucceed()
    {
        var user = CreateTestUser();
        var shelterId = Guid.NewGuid();
        var sub = ShelterSubscription.Create(user.Id, shelterId);
        user.AddShelterSubscription(sub);

        var result = user.RemoveShelterSubscription(shelterId);
        Assert.True(result);
        Assert.DoesNotContain(user.ShelterSubscriptions, s => s.ShelterId == shelterId);
    }

    /// <summary>
    /// Verifies that attempting to update a non-existent subscription returns false.
    /// </summary>
    [Fact]
    public void UpdateShelterSubscriptionDate_NotFound_ShouldReturnFalse()
    {
        var user = CreateTestUser();
        var result = user.UpdateShelterSubscriptionDate(Guid.NewGuid(), DateTime.UtcNow);
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that setting the last login date updates the property accordingly.
    /// </summary>
    [Fact]
    public void SetLastLogin_ShouldUpdateLastLogin()
    {
        var user = CreateTestUser();
        var now = DateTime.UtcNow;
        user.SetLastLogin(now);
        Assert.Equal(now, user.LastLogin);
    }

    /// <summary>
    /// Creates a test user with predefined valid values.
    /// </summary>
    /// <returns>A new instance of <see cref="User"/> for testing.</returns>
    private static User CreateTestUser() =>
       User.Create(
           email: "user@example.com",
           passwordHash: "hashed-password",
           firstName: "John",
           lastName: "Doe",
           phone: "+380501234567",
           role: UserRole.User);
}