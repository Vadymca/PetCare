// <copyright file="VolunteerTaskAssignment.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;

/// <summary>
/// Represents a value object that associates a user with a volunteer task assignment.
/// </summary>
public sealed class VolunteerTaskAssignment : ValueObject
{
    private VolunteerTaskAssignment()
    {
    }

    private VolunteerTaskAssignment(
        Guid volunteerTaskId,
        Guid userId,
        DateTime assignedAt)
    {
        if (volunteerTaskId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор завдання волонтера не може бути порожнім.", nameof(volunteerTaskId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        this.VolunteerTaskId = volunteerTaskId;
        this.UserId = userId;
        this.AssignedAt = assignedAt;
    }

    /// <summary>
    /// Gets the unique identifier of the volunteer task.
    /// </summary>
    public Guid VolunteerTaskId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user assigned to the task.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the date and time when the task was assigned to the user.
    /// </summary>
    public DateTime AssignedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="VolunteerTaskAssignment"/> instance with the specified parameters.
    /// </summary>
    /// <param name="volunteerTaskId">The unique identifier of the volunteer task.</param>
    /// <param name="userId">The unique identifier of the user assigned to the task.</param>
    /// <returns>A new instance of <see cref="VolunteerTaskAssignment"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="volunteerTaskId"/> or <paramref name="userId"/> is empty.</exception>
    public static VolunteerTaskAssignment Create(Guid volunteerTaskId, Guid userId) =>
        new VolunteerTaskAssignment(volunteerTaskId, userId, DateTime.UtcNow);

    /// <summary>
    /// Returns the components used to determine equality for the <see cref="VolunteerTaskAssignment"/> instance.
    /// </summary>
    /// <returns>An enumerable of objects representing the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.VolunteerTaskId;
        yield return this.UserId;
    }
}
