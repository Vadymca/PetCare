// <copyright file="VolunteerTask.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using NetTopologySuite.Geometries;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a volunteer task in the system.
/// </summary>
public sealed class VolunteerTask : BaseEntity
{
    private VolunteerTask()
    {
        this.Title = Title.Create(string.Empty);
        this.SkillsRequired = new Dictionary<string, string>();
    }

    private VolunteerTask(
        Guid shelterId,
        Title title,
        string? description,
        DateOnly date,
        int? duration,
        int requiredVolunteers,
        VolunteerTaskStatus status,
        int pointsReward,
        Point? location,
        Dictionary<string, string> skillsRequired)
    {
        if (requiredVolunteers <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(requiredVolunteers), "Повинно бути більше нуля");
        }

        if (duration.HasValue && duration <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "Якщо вказано, має бути більше нуля.");
        }

        if (pointsReward < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pointsReward), "Не може бути негативним.");
        }

        this.ShelterId = shelterId;
        this.Title = title;
        this.Description = description?.Trim();
        this.Date = date;
        this.Duration = duration;
        this.RequiredVolunteers = requiredVolunteers;
        this.Status = status;
        this.PointsReward = pointsReward;
        this.Location = location;
        this.SkillsRequired = skillsRequired;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of the shelter associated with the task.
    /// </summary>
    public Guid ShelterId { get; private set; }

    /// <summary>
    /// Gets the title of the volunteer task.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Gets the description of the volunteer task, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the date of the volunteer task.
    /// </summary>
    public DateOnly Date { get; private set; }

    /// <summary>
    /// Gets the duration of the task in minutes, if specified. Can be null.
    /// </summary>
    public int? Duration { get; private set; }

    /// <summary>
    /// Gets the number of volunteers required for the task.
    /// </summary>
    public int RequiredVolunteers { get; private set; }

    /// <summary>
    /// Gets the status of the volunteer task.
    /// </summary>
    public VolunteerTaskStatus Status { get; private set; }

    /// <summary>
    /// Gets the points awarded for completing the task.
    /// </summary>
    public int PointsReward { get; private set; }

    /// <summary>
    /// Gets the geographic location of the task, if any. Can be null.
    /// </summary>
    public Point? Location { get; private set; }

    /// <summary>
    /// Gets the skills required for the task.
    /// </summary>
    public Dictionary<string, string> SkillsRequired { get; private set; }

    /// <summary>
    /// Gets the date and time when the volunteer task was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the volunteer task was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="VolunteerTask"/> instance with the specified parameters.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter associated with the task.</param>
    /// <param name="title">The title of the volunteer task.</param>
    /// <param name="description">The description of the volunteer task, if any. Can be null.</param>
    /// <param name="date">The date of the volunteer task.</param>
    /// <param name="duration">The duration of the task in minutes, if specified. Can be null.</param>
    /// <param name="requiredVolunteers">The number of volunteers required for the task.</param>
    /// <param name="status">The status of the volunteer task.</param>
    /// <param name="pointsReward">The points awarded for completing the task.</param>
    /// <param name="location">The geographic location of the task, if any. Can be null.</param>
    /// <param name="skillsRequired">The skills required for the task, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="VolunteerTask"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="requiredVolunteers"/> is less than or equal to zero, <paramref name="duration"/> is specified and less than or equal to zero, or <paramref name="pointsReward"/> is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to the <see cref="Title.Create"/> method.</exception>
    public static VolunteerTask Create(
        Guid shelterId,
        string title,
        string? description,
        DateOnly date,
        int? duration,
        int requiredVolunteers,
        VolunteerTaskStatus status,
        int pointsReward,
        Point? location,
        Dictionary<string, string>? skillsRequired)
    {
        return new VolunteerTask(
            shelterId,
            Title.Create(title),
            description,
            date,
            duration,
            requiredVolunteers,
            status,
            pointsReward,
            location,
            skillsRequired ?? new Dictionary<string, string>());
    }

    /// <summary>
    /// Updates the status of the volunteer task.
    /// </summary>
    /// <param name="newStatus">The new status of the volunteer task.</param>
    public void UpdateStatus(VolunteerTaskStatus newStatus)
    {
        this.Status = newStatus;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the information of the volunteer task with the provided values.
    /// </summary>
    /// <param name="title">The new title of the volunteer task.</param>
    /// <param name="description">The new description of the volunteer task, if any. Can be null.</param>
    /// <param name="date">The new date of the volunteer task.</param>
    /// <param name="duration">The new duration of the task in minutes, if specified. Can be null.</param>
    /// <param name="requiredVolunteers">The new number of volunteers required for the task.</param>
    /// <param name="pointsReward">The new points awarded for completing the task.</param>
    /// <param name="location">The new geographic location of the task, if any. Can be null.</param>
    /// <param name="skillsRequired">The new skills required for the task, if any. Can be null.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="requiredVolunteers"/> is less than or equal to zero, <paramref name="duration"/> is specified and less than or equal to zero, or <paramref name="pointsReward"/> is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to the <see cref="Title.Create"/> method.</exception>
    public void UpdateInfo(
        string title,
        string? description,
        DateOnly date,
        int? duration,
        int requiredVolunteers,
        int pointsReward,
        Point? location,
        Dictionary<string, string>? skillsRequired)
    {
        if (requiredVolunteers <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(requiredVolunteers));
        }

        if (duration.HasValue && duration <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(duration));
        }

        if (pointsReward < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pointsReward));
        }

        this.Title = Title.Create(title);
        this.Description = description?.Trim();
        this.Date = date;
        this.Duration = duration;
        this.RequiredVolunteers = requiredVolunteers;
        this.PointsReward = pointsReward;
        this.Location = location;
        this.SkillsRequired = skillsRequired ?? new Dictionary<string, string>();
        this.UpdatedAt = DateTime.UtcNow;
    }
}
