// <copyright file="VolunteerTaskTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="VolunteerTask"/> aggregate.
/// </summary>
public sealed class VolunteerTaskTests
{
    private static readonly Guid ShelterId = Guid.NewGuid();

    /// <summary>
    /// Tests that creating a volunteer task with valid data succeeds.
    /// </summary>
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var skills = new Dictionary<string, string> { { "Skill1", "Description1" } };
        var task = VolunteerTask.Create(
            ShelterId,
            "Help Animals",
            "Assist with animal care",
            date,
            120,
            5,
            VolunteerTaskStatus.Open,
            50,
            null,
            skills);

        Assert.Equal(ShelterId, task.ShelterId);
        Assert.Equal("Help Animals", task.Title.Value);
        Assert.Equal("Assist with animal care", task.Description);
        Assert.Equal(date, task.Date);
        Assert.Equal(120, task.Duration);
        Assert.Equal(5, task.RequiredVolunteers);
        Assert.Equal(VolunteerTaskStatus.Open, task.Status);
        Assert.Equal(50, task.PointsReward);
        Assert.NotNull(task.SkillsRequired);
        Assert.Equal("Description1", task.SkillsRequired["Skill1"]);
    }

    /// <summary>
    /// Tests that creating with invalid RequiredVolunteers throws.
    /// </summary>
    [Fact]
    public void Create_WithNonPositiveRequiredVolunteers_ShouldThrow()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            VolunteerTask.Create(
                ShelterId,
                "Title",
                null,
                date,
                null,
                0,
                VolunteerTaskStatus.Open,
                10,
                null,
                null));
    }

    /// <summary>
    /// Tests that creating with non-positive duration throws.
    /// </summary>
    [Fact]
    public void Create_WithNonPositiveDuration_ShouldThrow()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            VolunteerTask.Create(
                ShelterId,
                "Title",
                null,
                date,
                0,
                1,
                VolunteerTaskStatus.Open,
                10,
                null,
                null));
    }

    /// <summary>
    /// Tests that creating with negative points reward throws.
    /// </summary>
    [Fact]
    public void Create_WithNegativePointsReward_ShouldThrow()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            VolunteerTask.Create(
                ShelterId,
                "Title",
                null,
                date,
                null,
                1,
                VolunteerTaskStatus.Open,
                -1,
                null,
                null));
    }

    /// <summary>
    /// Tests updating the status of the volunteer task.
    /// </summary>
    [Fact]
    public void UpdateStatus_ShouldChangeStatusAndUpdateUpdatedAt()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            10,
            null,
            null);

        var beforeUpdate = task.UpdatedAt;

        task.UpdateStatus(VolunteerTaskStatus.Cancelled);

        Assert.Equal(VolunteerTaskStatus.Cancelled, task.Status);
        Assert.True(task.UpdatedAt > beforeUpdate);
    }

    /// <summary>
    /// Tests updating info with valid data.
    /// </summary>
    [Fact]
    public void UpdateInfo_WithValidData_ShouldUpdateFields()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var newDate = date.AddDays(1);
        var task = VolunteerTask.Create(
            ShelterId,
            "Initial Title",
            "Initial description",
            date,
            30,
            2,
            VolunteerTaskStatus.Open,
            5,
            null,
            new Dictionary<string, string> { { "OldSkill", "OldDesc" } });

        var newSkills = new Dictionary<string, string> { { "NewSkill", "NewDesc" } };

        var location = Coordinates.From(45.0, 90.0);

        task.UpdateInfo(
            "Updated Title",
            "Updated description",
            newDate,
            60,
            3,
            10,
            location,
            newSkills);

        Assert.Equal("Updated Title", task.Title.Value);
        Assert.Equal("Updated description", task.Description);
        Assert.Equal(newDate, task.Date);
        Assert.Equal(60, task.Duration);
        Assert.Equal(3, task.RequiredVolunteers);
        Assert.Equal(10, task.PointsReward);
        Assert.NotNull(task.Location);
        Assert.Equal(45.0, task.Location!.Latitude);
        Assert.Equal(90.0, task.Location.Longitude);
        Assert.Equal(newSkills, task.SkillsRequired);
    }

    /// <summary>
    /// Tests that updating info with invalid RequiredVolunteers throws.
    /// </summary>
    [Fact]
    public void UpdateInfo_WithNonPositiveRequiredVolunteers_ShouldThrow()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            10,
            null,
            null);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            task.UpdateInfo(
                "Title",
                null,
                date,
                null,
                0,
                10,
                null,
                null));
    }

    /// <summary>
    /// Tests that updating info with non-positive duration throws.
    /// </summary>
    [Fact]
    public void UpdateInfo_WithNonPositiveDuration_ShouldThrow()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            10,
            1,
            VolunteerTaskStatus.Open,
            10,
            null,
            null);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            task.UpdateInfo(
                "Title",
                null,
                date,
                0,
                1,
                10,
                null,
                null));
    }

    /// <summary>
    /// Tests that updating info with negative points reward throws.
    /// </summary>
    [Fact]
    public void UpdateInfo_WithNegativePointsReward_ShouldThrow()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            10,
            null,
            null);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            task.UpdateInfo(
                "Title",
                null,
                date,
                null,
                1,
                -1,
                null,
                null));
    }

    /// <summary>
    /// Tests adding a new skill requirement.
    /// </summary>
    [Fact]
    public void AddOrUpdateSkill_NewSkill_ShouldAdd()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            0,
            null,
            null);

        task.AddOrUpdateSkill("Skill1", "Description1");

        Assert.True(task.SkillsRequired.ContainsKey("Skill1"));
        Assert.Equal("Description1", task.SkillsRequired["Skill1"]);
    }

    /// <summary>
    /// Tests updating an existing skill requirement.
    /// </summary>
    [Fact]
    public void AddOrUpdateSkill_ExistingSkill_ShouldUpdate()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var skills = new Dictionary<string, string> { { "Skill1", "Desc1" } };
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            0,
            null,
            skills);

        task.AddOrUpdateSkill("Skill1", "UpdatedDesc");

        Assert.Equal("UpdatedDesc", task.SkillsRequired["Skill1"]);
    }

    /// <summary>
    /// Tests that <see cref="VolunteerTask.AddOrUpdateSkill(string, string)"/>
    /// throws <see cref="ArgumentException"/> when the skill name is null,
    /// empty, or whitespace.
    /// </summary>
    /// <param name="skillName">The invalid skill name to test.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void AddOrUpdateSkill_InvalidSkillName_ShouldThrow(string? skillName)
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            0,
            null,
            null);

        Assert.Throws<ArgumentException>(() => task.AddOrUpdateSkill(skillName!, "desc"));
    }

    /// <summary>
    /// Tests removing an existing skill requirement.
    /// </summary>
    [Fact]
    public void RemoveSkill_ExistingSkill_ShouldRemoveAndReturnTrue()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var skills = new Dictionary<string, string> { { "Skill1", "Desc1" } };
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            0,
            null,
            skills);

        var removed = task.RemoveSkill("Skill1");

        Assert.True(removed);
        Assert.DoesNotContain("Skill1", task.SkillsRequired.Keys);
    }

    /// <summary>
    /// Tests removing a non-existing skill requirement returns false.
    /// </summary>
    [Fact]
    public void RemoveSkill_NonExistingSkill_ShouldReturnFalse()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            0,
            null,
            null);

        var removed = task.RemoveSkill("NonExisting");

        Assert.False(removed);
    }

    /// <summary>
    /// Tests that <see cref="VolunteerTask.RemoveSkill(string)"/>
    /// returns false when the skill name is null, empty, or whitespace.
    /// </summary>
    /// <param name="skillName">The invalid skill name to test.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void RemoveSkill_InvalidSkillName_ShouldReturnFalse(string? skillName)
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var task = VolunteerTask.Create(
            ShelterId,
            "Title",
            null,
            date,
            null,
            1,
            VolunteerTaskStatus.Open,
            0,
            null,
            null);

        var removed = task.RemoveSkill(skillName!);

        Assert.False(removed);
    }
}
