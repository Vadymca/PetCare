namespace PetCare.Tests.Domain.Specifications;

using System;
using System.Collections.Generic;
using System.Linq;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.Specifications.VolunteerTask;

/// <summary>
/// Contains unit tests for <see cref="VolunteerTask"/> specifications.
/// Tests filtering by date and shelter ID.
/// </summary>
public class VolunteerTaskSpecificationTests
{
    /// <summary>
    /// Tests that <see cref="VolunteerTasksByDateSpecification"/> correctly filters tasks by date.
    /// </summary>
    [Fact]
    public void VolunteerTasksByDateSpecification_ShouldFilterByDate()
    {
        // Arrange
        var date1 = new DateOnly(2025, 8, 19);
        var date2 = new DateOnly(2025, 8, 20);
        var task1 = CreateTask(Guid.NewGuid(), date1);
        var task2 = CreateTask(Guid.NewGuid(), date2);
        var tasks = new List<VolunteerTask> { task1, task2 };

        var spec = new VolunteerTasksByDateSpecification(date2);

        // Act
        var result = tasks.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(date2, result[0].Date);
    }

    /// <summary>
    /// Tests that <see cref="VolunteerTasksByShelterSpecification"/> correctly filters tasks by shelter ID.
    /// </summary>
    [Fact]
    public void VolunteerTasksByShelterSpecification_ShouldFilterByShelter()
    {
        // Arrange
        var shelterId1 = Guid.NewGuid();
        var shelterId2 = Guid.NewGuid();
        var task1 = CreateTask(shelterId1, new DateOnly(2025, 8, 19));
        var task2 = CreateTask(shelterId2, new DateOnly(2025, 8, 19));
        var tasks = new List<VolunteerTask> { task1, task2 };

        var spec = new VolunteerTasksByShelterSpecification(shelterId1);

        // Act
        var result = tasks.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(shelterId1, result[0].ShelterId);
    }

    /// <summary>
    /// Tests that <see cref="VolunteerTasksByShelterSpecification"/> throws an exception when shelter ID is empty.
    /// </summary>
    [Fact]
    public void VolunteerTasksByShelterSpecification_ShouldThrow_WhenShelterIdEmpty()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new VolunteerTasksByShelterSpecification(Guid.Empty));
        Assert.Contains("Ідентифікатор притулку не може бути порожнім", ex.Message);
    }

    /// <summary>
    /// Helper method to create a volunteer task with minimal required parameters.
    /// </summary>
    /// <param name="shelterId">The shelter ID.</param>
    /// <param name="date">The task date.</param>
    /// <returns>A new <see cref="VolunteerTask"/> instance.</returns>
    private static VolunteerTask CreateTask(Guid shelterId, DateOnly date)
    {
        return VolunteerTask.Create(
            shelterId,
            "Task title",
            "Task description",
            date,
            duration: 60,
            requiredVolunteers: 5,
            VolunteerTaskStatus.Open,
            pointsReward: 10,
            location: null,
            skillsRequired: null);
    }
}
