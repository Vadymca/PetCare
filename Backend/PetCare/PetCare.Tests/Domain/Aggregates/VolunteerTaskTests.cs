namespace PetCare.Tests.Domain.Aggregates;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// Тести для агрегату <see cref="VolunteerTask"/>.
/// </summary>
public class VolunteerTaskTests
{
    private readonly Guid shelterId = Guid.NewGuid();
    private readonly DateOnly taskDate = DateOnly.FromDateTime(DateTime.UtcNow);

    /// <summary>
    /// Тестує створення завдання волонтера з валідними параметрами.
    /// </summary>
    [Fact]
    public void Create_ValidParameters_CreatesVolunteerTask()
    {
        // Arrange
        string title = "Volunteer Cleanup";
        string description = "Clean the shelter";
        int? duration = 120;
        int requiredVolunteers = 5;
        VolunteerTaskStatus status = VolunteerTaskStatus.Open;
        int pointsReward = 50;
        var location = Coordinates.From(50.0, 30.0);
        var skillsRequired = new Dictionary<string, string> { { "Cleaning", "Basic cleaning skills" } };

        // Act
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: title,
            description: description,
            date: this.taskDate,
            duration: duration,
            requiredVolunteers: requiredVolunteers,
            status: status,
            pointsReward: pointsReward,
            location: location,
            skillsRequired: skillsRequired);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(this.shelterId, task.ShelterId);
        Assert.Equal(title, task.Title.Value);
        Assert.Equal(description, task.Description);
        Assert.Equal(this.taskDate, task.Date);
        Assert.Equal(duration, task.Duration);
        Assert.Equal(requiredVolunteers, task.RequiredVolunteers);
        Assert.Equal(status, task.Status);
        Assert.Equal(pointsReward, task.PointsReward);
        Assert.Equal(location, task.Location);
        Assert.Equal(skillsRequired, task.SkillsRequired);
        Assert.True(task.CreatedAt <= DateTime.UtcNow);
        Assert.InRange(task.UpdatedAt, task.CreatedAt, task.CreatedAt.AddMilliseconds(100)); // Допустима похибка 100 мс
    }

    /// <summary>
    /// Тестує створення завдання волонтера з невалідною кількістю волонтерів.
    /// </summary>
    [Fact]
    public void Create_ZeroOrNegativeVolunteers_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        string title = "Volunteer Cleanup";
        int requiredVolunteers = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => VolunteerTask.Create(
                shelterId: this.shelterId,
                title: title,
                description: null,
                date: this.taskDate,
                duration: null,
                requiredVolunteers: requiredVolunteers,
                status: VolunteerTaskStatus.Open,
                pointsReward: 50,
                location: null,
                skillsRequired: null));
    }

    /// <summary>
    /// Тестує створення завдання волонтера з невалідною тривалістю.
    /// </summary>
    [Fact]
    public void Create_NegativeDuration_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        string title = "Volunteer Cleanup";
        int? duration = -60;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => VolunteerTask.Create(
                shelterId: this.shelterId,
                title: title,
                description: null,
                date: this.taskDate,
                duration: duration,
                requiredVolunteers: 5,
                status: VolunteerTaskStatus.Open,
                pointsReward: 50,
                location: null,
                skillsRequired: null));
    }

    /// <summary>
    /// Тестує створення завдання волонтера з негативною кількістю балів.
    /// </summary>
    [Fact]
    public void Create_NegativePointsReward_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        string title = "Volunteer Cleanup";
        int pointsReward = -50;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => VolunteerTask.Create(
                shelterId: this.shelterId,
                title: title,
                description: null,
                date: this.taskDate,
                duration: null,
                requiredVolunteers: 5,
                status: VolunteerTaskStatus.Open,
                pointsReward: pointsReward,
                location: null,
                skillsRequired: null));
    }

    /// <summary>
    /// Тестує оновлення статусу завдання волонтера.
    /// </summary>
    [Fact]
    public void UpdateStatus_ValidStatus_UpdatesStatusAndTimestamp()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: null,
            date: this.taskDate,
            duration: null,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: null);
        var newStatus = VolunteerTaskStatus.InProgress;
        var initialUpdatedAt = task.UpdatedAt;

        // Act
        task.UpdateStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, task.Status);
        Assert.True(task.UpdatedAt > initialUpdatedAt);
    }

    /// <summary>
    /// Тестує оновлення інформації про завдання волонтера.
    /// </summary>
    [Fact]
    public void UpdateInfo_ValidParameters_UpdatesTaskInfo()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: "Initial description",
            date: this.taskDate,
            duration: 60,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: new Dictionary<string, string> { { "InitialSkill", "Initial description" } });
        string newTitle = "Updated Cleanup";
        string newDescription = "Updated description";
        var newDate = this.taskDate.AddDays(1);
        int? newDuration = 120;
        int newRequiredVolunteers = 10;
        int newPointsReward = 100;
        var newLocation = Coordinates.From(51.0, 31.0);
        var newSkillsRequired = new Dictionary<string, string> { { "NewSkill", "New description" } };
        var initialUpdatedAt = task.UpdatedAt;

        // Act
        task.UpdateInfo(
            title: newTitle,
            description: newDescription,
            date: newDate,
            duration: newDuration,
            requiredVolunteers: newRequiredVolunteers,
            pointsReward: newPointsReward,
            location: newLocation,
            skillsRequired: newSkillsRequired);

        // Assert
        Assert.Equal(newTitle, task.Title.Value);
        Assert.Equal(newDescription, task.Description);
        Assert.Equal(newDate, task.Date);
        Assert.Equal(newDuration, task.Duration);
        Assert.Equal(newRequiredVolunteers, task.RequiredVolunteers);
        Assert.Equal(newPointsReward, task.PointsReward);
        Assert.Equal(newLocation, task.Location);
        Assert.Equal(newSkillsRequired, task.SkillsRequired);
        Assert.True(task.UpdatedAt > initialUpdatedAt);
    }

    /// <summary>
    /// Тестує оновлення інформації з невалідною кількістю волонтерів.
    /// </summary>
    [Fact]
    public void UpdateInfo_ZeroOrNegativeVolunteers_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: null,
            date: this.taskDate,
            duration: null,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: null);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => task.UpdateInfo(
                title: "Updated Cleanup",
                description: null,
                date: this.taskDate,
                duration: null,
                requiredVolunteers: 0,
                pointsReward: 50,
                location: null,
                skillsRequired: null));
    }

    /// <summary>
    /// Тестує додавання нової навички до завдання волонтера.
    /// </summary>
    [Fact]
    public void AddOrUpdateSkill_ValidSkill_AddsSkillAndUpdatesTimestamp()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: null,
            date: this.taskDate,
            duration: null,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: null);
        string skillName = "Cleaning";
        string skillDescription = "Basic cleaning skills";
        var initialUpdatedAt = task.UpdatedAt;

        // Act
        task.AddOrUpdateSkill(skillName, skillDescription);

        // Assert
        Assert.Equal(skillDescription, task.SkillsRequired[skillName]);
        Assert.True(task.UpdatedAt > initialUpdatedAt);
        Assert.Single(task.SkillsRequired);
    }

    /// <summary>
    /// Тестує оновлення існуючої навички завдання волонтера.
    /// </summary>
    [Fact]
    public void AddOrUpdateSkill_ExistingSkill_UpdatesSkillDescription()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: null,
            date: this.taskDate,
            duration: null,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: new Dictionary<string, string> { { "Cleaning", "Old description" } });
        string skillName = "Cleaning";
        string newSkillDescription = "Updated cleaning skills";
        var initialUpdatedAt = task.UpdatedAt;

        // Act
        task.AddOrUpdateSkill(skillName, newSkillDescription);

        // Assert
        Assert.Equal(newSkillDescription, task.SkillsRequired[skillName]);
        Assert.True(task.UpdatedAt > initialUpdatedAt);
        Assert.Single(task.SkillsRequired);
    }

    /// <summary>
    /// Тестує додавання навички з невалідною назвою.
    /// </summary>
    [Fact]
    public void AddOrUpdateSkill_NullOrWhitespaceSkillName_ThrowsArgumentException()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: null,
            date: this.taskDate,
            duration: null,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: null);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => task.AddOrUpdateSkill(null!, "Description"));
        Assert.Throws<ArgumentException>(() => task.AddOrUpdateSkill(string.Empty, "Description"));
        Assert.Throws<ArgumentException>(() => task.AddOrUpdateSkill(" ", "Description"));
    }

    /// <summary>
    /// Тестує видалення існуючої навички.
    /// </summary>
    [Fact]
    public void RemoveSkill_ExistingSkill_RemovesSkillAndReturnsTrue()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: null,
            date: this.taskDate,
            duration: null,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: new Dictionary<string, string> { { "Cleaning", "Basic cleaning skills" } });
        string skillName = "Cleaning";
        var initialUpdatedAt = task.UpdatedAt;

        // Act
        bool result = task.RemoveSkill(skillName);

        // Assert
        Assert.True(result);
        Assert.False(task.SkillsRequired.ContainsKey(skillName));
        Assert.True(task.UpdatedAt > initialUpdatedAt);
        Assert.Empty(task.SkillsRequired);
    }

    /// <summary>
    /// Тестує видалення неіснуючої навички.
    /// </summary>
    [Fact]
    public void RemoveSkill_NonExistingSkill_ReturnsFalse()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: null,
            date: this.taskDate,
            duration: null,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: null);
        string skillName = "NonExistingSkill";
        var initialUpdatedAt = task.UpdatedAt;

        // Act
        bool result = task.RemoveSkill(skillName);

        // Assert
        Assert.False(result);
        Assert.Equal(initialUpdatedAt, task.UpdatedAt);
    }

    /// <summary>
    /// Тестує видалення навички з невалідною назвою.
    /// </summary>
    [Fact]
    public void RemoveSkill_NullOrWhitespaceSkillName_ReturnsFalse()
    {
        // Arrange
        var task = VolunteerTask.Create(
            shelterId: this.shelterId,
            title: "Volunteer Cleanup",
            description: null,
            date: this.taskDate,
            duration: null,
            requiredVolunteers: 5,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: null);

        // Act
        bool result1 = task.RemoveSkill(null!);
        bool result2 = task.RemoveSkill(string.Empty);
        bool result3 = task.RemoveSkill(" ");

        // Assert
        Assert.False(result1);
        Assert.False(result2);
        Assert.False(result3);
        Assert.InRange(task.UpdatedAt, task.CreatedAt, task.CreatedAt.AddMilliseconds(100));
    }
}
